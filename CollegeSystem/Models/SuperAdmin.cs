namespace CollegeSystem.Models
{
    public class SuperAdmin : AppUser
    {
        // Navigation Properties
        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
    }
}