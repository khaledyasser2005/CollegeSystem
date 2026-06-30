namespace CollegeSystem.Models
{
    public class Professor : AppUser
    {
        // Navigation Properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}