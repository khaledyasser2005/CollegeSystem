namespace CollegeSystem.Models
{
    public class Student : AppUser
    {
        public string Phone { get; set; }
        public double GPA { get; set; }
        public int Level { get; set; }
        public int DepartmentID { get; set; }
        public int CompletedHours { get; set; }

        // Navigation Properties
        public Department Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}