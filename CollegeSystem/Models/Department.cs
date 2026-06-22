namespace CollegeSystem.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double GPARequired { get; set; }

        // Navigation Properties
        public ICollection<Student> Students { get; set; }
    }
}