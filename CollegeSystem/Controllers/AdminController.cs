using CollegeSystem.Data;
using CollegeSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace CollegeSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }
        public IActionResult ManageProfessors()
        {
            return View("ManageProfessors");
        }
        [HttpPost]
        public IActionResult AddProfessor(Professor model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View("ManageProfessors");
            }
            var _exists = _context.Professors.Any(p => p.Email == model.Email);
            if(_exists)
            {
                ModelState.AddModelError("", "Email Already Exists!");
                return View("ManageProfessors");
            }
            Professor professor = new Professor();
            professor.Email = model.Email;
            professor.Name = model.Name;

            var hasher = new PasswordHasher<Professor>();
            professor.Password = hasher.HashPassword(professor,model.Password);

            _context.Professors.Add(professor);
            _context.SaveChanges();
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult ManageCourses()
        {
            return View("ManageCourses");
        }
        public IActionResult ManageStudents()
        {
            return View("ManageStudents");

        }
        public IActionResult CollegeCourses()
        {
            return View("CollegeCourses");
        }

        public IActionResult AssignCourseToStudent()
        {
            return View("AssignCourseToStudent");
        }

        public IActionResult AssignCourseToProfessor()
        {
            return View("AssignCourseToProfessor");
        }
    }
}