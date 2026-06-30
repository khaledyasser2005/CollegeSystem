namespace CollegeSystem.Models
{
    public class Admin : AppUser
    {
        // Navigation Properties
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}