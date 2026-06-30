namespace CollegeSystem.Models
{
    public class Material
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime UploadDate { get; set; }
        public int CourseID { get; set; }

        // Navigation Properties
        public Course Course { get; set; } = null!;
    }
}