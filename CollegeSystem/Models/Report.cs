namespace CollegeSystem.Models
{
    public class Report
    {
        public int ID { get; set; }

        public string Title { get; set; } = null!;

        public string Type { get; set; } = "PDF";

        public string Status { get; set; } = "Pending";

        public DateTime GeneratedDate { get; set; }

        public string GeneratedBy { get; set; } = null!;

        //public int AdminID { get; set; }

        // 📌 PDF file
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
        public int CourseID { get; set; }

        public Course Course { get; set; } = null!;



        //public Admin Admin { get; set; }
    }
}