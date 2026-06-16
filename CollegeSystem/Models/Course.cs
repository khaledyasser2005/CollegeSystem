namespace CollegeSystem.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Semesters { get; set; }
        public string CoursePrerequisites { get; set; }
        public int ProfessorID { get; set; }

        // Navigation Properties
        public Professor Professor { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<Material> Materials { get; set; }
    }
}