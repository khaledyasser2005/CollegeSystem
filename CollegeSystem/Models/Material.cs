namespace CollegeSystem.Models
{
    public class Material
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string Type { get; set; }
        public DateTime UploadDate { get; set; }
        public int CourseID { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
    }
}