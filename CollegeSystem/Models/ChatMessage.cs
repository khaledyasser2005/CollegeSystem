namespace CollegeSystem.Models
{
    /// <summary>
    /// Persisted chat message exchanged between a user and the AI assistant.
    /// </summary>
    public class ChatMessage
    {
        public int ID { get; set; }

        /// <summary>
        /// The role of the message sender as stored in session: Student, Professor, Admin, Super Admin.
        /// </summary>
        public string UserRole { get; set; } = string.Empty;

        /// <summary>
        /// The numeric user ID as stored in session (maps to Student.ID / Professor.ID / etc.).
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// "user" or "assistant" — matches Gemini API role conventions.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
