namespace CollegeSystem.Models
{
    public class Admin : AppUser
    {
        // Navigation Properties
        public Department Department { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}