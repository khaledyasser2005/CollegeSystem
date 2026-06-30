namespace CollegeSystem.Models
{
    public class Enrollment
    {
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public string? Grade { get; set; }
        public string? Semester { get; set; }

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}