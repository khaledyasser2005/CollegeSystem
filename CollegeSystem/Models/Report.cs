namespace CollegeSystem.Models
{
    public class Report
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Type { get; set; } = "PDF";

        public string Status { get; set; } = "Pending";

        public DateTime GeneratedDate { get; set; }

        public string GeneratedBy { get; set; }

        public int AdminID { get; set; }

        // 📌 PDF file
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public int CourseID { get; set; }

        public Course Course { get; set; }



        public Admin Admin { get; set; }
    }
}