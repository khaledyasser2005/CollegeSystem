namespace CollegeSystem.Models
{
    public class Assignment
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int MaxMarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CourseID { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
    }
}