using Microsoft.AspNetCore.Mvc;
using CollegeSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CollegeSystem.Controllers
{
    public class StudentController : Controller
    {

        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult MyCourses()
        {
            var studentId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(studentId))
                return RedirectToAction("Login", "Account");

            int id = int.Parse(studentId);

            var courses = _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentID == id)
                .Select(e => e.Course)
                .ToList();

            return View(courses);
        }
        public IActionResult CourseDetails(int id)
        {
            var studentId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(studentId))
                return RedirectToAction("Login", "Account");

            int currentStudentId = int.Parse(studentId);

            var isEnrolled = _context.Enrollments
                .Any(e => e.StudentID == currentStudentId && e.CourseID == id);

            if (!isEnrolled)
                return RedirectToAction("MyCourses");

            var course = _context.Courses.FirstOrDefault(c => c.ID == id);

            if (course == null)
                return NotFound();

            ViewBag.Materials = _context.Materials
                .Where(m => m.CourseID == id)
                .OrderByDescending(m => m.UploadDate)
                .ToList();

            ViewBag.Assignments = _context.Assignments
                .Where(a => a.CourseID == id)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            ViewBag.Quizzes = _context.Quizzes
                .Where(q => q.CourseID == id)
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

            return View(course);
        }
        public IActionResult CollegeCourses()
        {
            var courses = _context.Courses.ToList();

            return View(courses);
        }
        public IActionResult Students()
        {
            return View();
        }
        public IActionResult Professors()
        {
            try
            {
                var professors = _context.Professors.ToList();
                return View(professors);
            }
            catch
            {
                ViewBag.ErrorMessage = "Unable to load professors right now.";
                return View(new List<CollegeSystem.Models.Professor>());
            }
        }
    }
}
