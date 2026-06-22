using System.ComponentModel.DataAnnotations;

namespace CollegeSystem.ViewModels
{
    public class  AddAdminModel
    {
        [Required(ErrorMessage = "Admin Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Admin Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Admin Name is required")]
        public string Name { get; set; }
    }
    public class UpdateAdminModel
    {
        [Required(ErrorMessage = "Admin ID is required")]
        public int ID {  get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
    }
    public class DeleteAdminModel
    {
        [Required(ErrorMessage = "Admin ID is required")]
        public int ID { get; set; }
    }
}
