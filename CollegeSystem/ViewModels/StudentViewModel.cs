using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CollegeSystem.ViewModels
{
    //write your code for Add,Update,Delete StudentViewModel
    //get ideas from ProfessorViewModel
    public class AddStudentViewModel
    {
        [Required(ErrorMessage = "Student Name is required")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Student Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Student Password is required")]
        public string Password {  get; set; } = null!;

        [Required(ErrorMessage = "Student Phone is required")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Student Level is required")]
        public int Level { get; set; }

        [Required(ErrorMessage = "Department ID is required")]
        public int DepartmentID {  get; set; }
    }

    public class UpdateStudentViewModel
    {

        [Required(ErrorMessage = "Student ID is required")]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }
        public int? Level { get; set; }

        public int? DepartmentID { get; set; }
    }

    public class DeleteStudentViewModel
    {
        public int ID { get; set;}
    }
}