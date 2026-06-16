namespace CollegeSystem.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public string ReceiverType { get; set; }
        public int AdminID { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; }
    }
}