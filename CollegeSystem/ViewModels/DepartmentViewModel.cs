using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CollegeSystem.ViewModels
{
    public class AddDepartmentViewModel
    {
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Department GPA is required")]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0 and 4")]
        public double GPARequired { get; set; }
    }
    public class UpdateDepartmentViewModel
    {
        [Required(ErrorMessage = "Department ID is Required")]
        public int ID {  get; set; }
        public string? Name { get; set; }
        public double? GPARequired { get; set; }
    }
    public class DeleteDepartmentViewModel
    {
        [Required (ErrorMessage = "Department ID is Required")]
        public int ID {  get; set; }
    }
}