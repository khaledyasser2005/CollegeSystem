using System.ComponentModel.DataAnnotations;

namespace CollegeSystem.ViewModels
{
    public class AddCourseViewModel
    {
        [Required(ErrorMessage = "Course name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Course Duration is required")]
        [Range(1, 10, ErrorMessage = "Duration must be between 1 and 10")]
        public int Duration { get; set; }
        public string? Semesters { get; set; }
        [Required(ErrorMessage = "Course Prerequists is required")]
        public string CoursePrerequisites { get; set; }
        public int? ProfessorID { get; set; }
    }
    public class UpdateCourseViewModel
    {
        [Required(ErrorMessage = "Course ID is required")]

        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? Semesters { get; set; }
        public string? CoursePrerequisites { get; set; }
        public int? ProfessorID { get; set; }
    }
    public class DeleteCourseViewModel
    {
        [Required(ErrorMessage = "Course ID is required")]
        public int ID { get; set; }
    }
}