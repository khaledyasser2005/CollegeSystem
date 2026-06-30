using CollegeSystem.Data;
using CollegeSystem.Models;
using CollegeSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeSystem.Controllers
{
    /// <summary>
    /// Handles all chat API endpoints. All actions require an active session.
    /// </summary>
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGeminiChatService _gemini;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            AppDbContext context,
            IGeminiChatService gemini,
            IConfiguration configuration,
            ILogger<ChatController> logger)
        {
            _context       = context;
            _gemini        = gemini;
            _configuration = configuration;
            _logger        = logger;
        }

        // ─────────────────────────────────────────────────────────────────
        // GET /Chat/History  — returns the last N messages for this user
        // ─────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var (userId, userRole) = GetSessionInfo();
            if (userId == null)
                return Json(new { error = "Not authenticated" });

            int maxMessages = _configuration.GetValue<int>("Gemini:MaxHistoryMessages", 20);

            var messages = await _context.ChatMessages
                .Where(m => m.UserId == userId.Value && m.UserRole == userRole)
                .OrderByDescending(m => m.Timestamp)
                .Take(maxMessages)
                .OrderBy(m => m.Timestamp)
                .Select(m => new { m.Role, m.Content, timestamp = m.Timestamp.ToString("o") })
                .ToListAsync();

            return Json(messages);
        }

        // ─────────────────────────────────────────────────────────────────
        // POST /Chat/Send  — sends a user message, returns AI reply
        // ─────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            var (userId, userRole) = GetSessionInfo();
            if (userId == null)
                return Json(new { error = "Not authenticated", success = false });

            if (string.IsNullOrWhiteSpace(request?.Message))
                return Json(new { error = "Message cannot be empty", success = false });

            string trimmedMessage = request.Message.Trim();
            if (trimmedMessage.Length > 2000)
                return Json(new { error = "Message is too long (max 2000 characters)", success = false });

            // Persist the user's message
            var userMsg = new ChatMessage
            {
                UserId    = userId.Value,
                UserRole  = userRole!,
                Role      = "user",
                Content   = trimmedMessage,
                Timestamp = DateTime.UtcNow
            };
            _context.ChatMessages.Add(userMsg);
            await _context.SaveChangesAsync();

            // Build contextual system prompt
            string systemPrompt = await BuildSystemPromptAsync(userId.Value, userRole!);

            // Fetch recent history to pass to Gemini (exclude the message just saved)
            int maxMessages = _configuration.GetValue<int>("Gemini:MaxHistoryMessages", 20);
            var history = await _context.ChatMessages
                .Where(m => m.UserId == userId.Value && m.UserRole == userRole && m.ID != userMsg.ID)
                .OrderByDescending(m => m.Timestamp)
                .Take(maxMessages - 1)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            // Call Gemini — catch all exceptions so a single failed AI call never crashes the page
            string assistantReply;
            try
            {
                assistantReply = await _gemini.GetResponseAsync(systemPrompt, history, trimmedMessage);
            }
           catch (Exception ex)
{
    _logger.LogError(ex, "Gemini Error");
    assistantReply = "I'm sorry, I encountered an error connecting to the AI service. Please try again in a moment.";
}

            // Persist the assistant's reply
            var assistantMsg = new ChatMessage
            {
                UserId    = userId.Value,
                UserRole  = userRole!,
                Role      = "assistant",
                Content   = assistantReply,
                Timestamp = DateTime.UtcNow
            };
            _context.ChatMessages.Add(assistantMsg);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success   = true,
                reply     = assistantReply,
                timestamp = assistantMsg.Timestamp.ToString("o")
            });
        }

        // ─────────────────────────────────────────────────────────────────
        // POST /Chat/Clear  — clears all chat history for this user
        // ─────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var (userId, userRole) = GetSessionInfo();
            if (userId == null)
                return Json(new { error = "Not authenticated", success = false });

            var messages = _context.ChatMessages
                .Where(m => m.UserId == userId.Value && m.UserRole == userRole);

            _context.ChatMessages.RemoveRange(messages);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ─────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────

        private (int? userId, string? userRole) GetSessionInfo()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var userRole  = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(userRole))
                return (null, null);

            if (!int.TryParse(userIdStr, out int userId))
                return (null, null);

            return (userId, userRole);
        }

        private async Task<string> BuildSystemPromptAsync(int userId, string userRole)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("You are an intelligent, helpful assistant embedded in the College CMS (Course Management System).");
            sb.AppendLine($"The user is a {userRole} in the system.");
            sb.AppendLine("Be concise, friendly, and accurate. Format your responses clearly using markdown when helpful.");
            sb.AppendLine("You can answer questions about courses, assignments, quizzes, materials, grades, departments, and general academic topics.");
            sb.AppendLine("Do not discuss topics unrelated to education or the college system.");
            sb.AppendLine();

            // Enrich context based on role — each branch only accesses data owned by this user
            switch (userRole)
            {
                case "Student":
                    var student = await _context.Students
                        .Include(s => s.Department)
                        .Include(s => s.Enrollments)
                            .ThenInclude(e => e.Course)
                        .FirstOrDefaultAsync(s => s.ID == userId);

                    if (student != null)
                    {
                        sb.AppendLine($"Student name: {student.Name}");
                        sb.AppendLine($"Email: {student.Email}");
                        sb.AppendLine($"Level: {student.Level}");
                        sb.AppendLine($"GPA: {student.GPA:F2}");
                        sb.AppendLine($"Department: {student.Department?.Name ?? "N/A"}");
                        sb.AppendLine($"Completed hours: {student.CompletedHours}");

                        if (student.Enrollments?.Any() == true)
                        {
                            sb.AppendLine("Enrolled courses:");
                            foreach (var e in student.Enrollments)
                            {
                                sb.AppendLine($"  - {e.Course?.Name} (Grade: {e.Grade ?? "N/A"}, Semester: {e.Semester ?? "N/A"})");
                            }
                        }
                        else
                        {
                            sb.AppendLine("The student is not enrolled in any courses.");
                        }
                    }
                    break;

                case "Professor":
                    var professor = await _context.Professors
                        .Include(p => p.Courses)
                        .FirstOrDefaultAsync(p => p.ID == userId);

                    if (professor != null)
                    {
                        sb.AppendLine($"Professor name: {professor.Name}");
                        sb.AppendLine($"Email: {professor.Email}");

                        if (professor.Courses?.Any() == true)
                        {
                            // Fetch all enrollment counts in a single query to avoid N+1
                            var courseIds = professor.Courses.Select(c => c.ID).ToList();
                            var enrollmentCounts = await _context.Enrollments
                                .Where(e => courseIds.Contains(e.CourseID))
                                .GroupBy(e => e.CourseID)
                                .Select(g => new { CourseID = g.Key, Count = g.Count() })
                                .ToDictionaryAsync(x => x.CourseID, x => x.Count);

                            sb.AppendLine("Courses taught:");
                            foreach (var c in professor.Courses)
                            {
                                int studentCount = enrollmentCounts.GetValueOrDefault(c.ID, 0);
                                sb.AppendLine($"  - {c.Name} ({studentCount} students enrolled)");
                            }
                        }
                        else
                        {
                            sb.AppendLine("The professor is not assigned to any courses.");
                        }
                    }
                    break;

                case "Admin":
                    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.ID == userId);
                    if (admin != null)
                    {
                        sb.AppendLine($"Admin name: {admin.Name}");
                        sb.AppendLine($"Email: {admin.Email}");
                    }
                    int totalStudents   = await _context.Students.CountAsync();
                    int totalProfessors = await _context.Professors.CountAsync();
                    int totalCourses    = await _context.Courses.CountAsync();
                    int totalDepts      = await _context.Departments.CountAsync();
                    sb.AppendLine($"System stats: {totalStudents} students, {totalProfessors} professors, {totalCourses} courses, {totalDepts} departments.");
                    break;

                case "Super Admin":
                    var superAdmin = await _context.SuperAdmins.FirstOrDefaultAsync(s => s.ID == userId);
                    if (superAdmin != null)
                    {
                        sb.AppendLine($"Super Admin name: {superAdmin.Name}");
                    }
                    int admins = await _context.Admins.CountAsync();
                    sb.AppendLine($"There are {admins} college admin(s) registered in the system.");
                    break;
            }

            return sb.ToString();
        }
    }

    public class SendMessageRequest
    {
        public string? Message { get; set; }
    }
}
