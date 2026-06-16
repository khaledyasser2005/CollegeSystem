namespace CollegeSystem.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string GeneratedBy { get; set; }
        public int AdminID { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; }
    }
}