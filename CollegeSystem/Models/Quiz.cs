namespace CollegeSystem.Models
{
    public class Quiz
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxScore { get; set; }
        public int TotalMarks { get; set; }
        public int Duration { get; set; }
        public int AttemptsAllowed { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CourseID { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
    }
}