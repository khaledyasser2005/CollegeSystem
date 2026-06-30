namespace CollegeSystem.Models
{
    public class Student : AppUser
    {
        public double GPA { get; set; }
        public string Phone { get; set; } = null!;
        public int Level { get; set; }
        public int DepartmentID { get; set; }
        public int CompletedHours { get; set; }

        // Navigation Properties
        public Department Department { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}