using System.ComponentModel.DataAnnotations;

namespace CollegeSystem.ViewModels
{
    public class AddProfessorViewModel
    {
        [Required(ErrorMessage = "Professor name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Professor Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Professor Password is required")]
        public string Password { get; set; }
    }
    public class UpdateProfessorViewModel
    {
        [Required(ErrorMessage = "Professor ID is required")]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class DeleteProfessorViewModel
    {
        [Required(ErrorMessage = "Professor ID is required")]
        public int ID { get; set; }
    }

}