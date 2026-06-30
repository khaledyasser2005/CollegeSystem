namespace CollegeSystem.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string ReceiverType { get; set; } = null!;
        public int AdminID { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; } = null!;
    }
}