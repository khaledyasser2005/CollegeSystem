using CollegeSystem.Data;
using Google.GenAI.Types;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace CollegeSystem.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        public ProfileController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            var Role = HttpContext.Session.GetString("Role");

            if(UserId == null || Role == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int id = int.Parse(UserId);
            if(Role == "Student")
            {
                var student = _context.Students
                        .Where(s => s.ID == id)
                        .Select(s => new
                        {
                            s.Name,
                            s.Email,
                            s.Level,
                            s.Phone,
                            s.GPA,
                            s.CompletedHours,
                            DepartmentName = s.Department.Name,
                            Courses = s.Enrollments.Select(e => e.Course.Name).ToList()
                        })
                        .FirstOrDefault(); 
                if (student == null) return NotFound();
                ViewBag.Name = student.Name;
                ViewBag.Email = student.Email;
                ViewBag.Level = student.Level;
                ViewBag.Phone = student.Phone;
                ViewBag.GPA = student.GPA;
                ViewBag.Department = student.DepartmentName;
                ViewBag.Courses = student.Courses;
            }
            else if(Role == "Admin")
            {
                var admin = _context.Admins.FirstOrDefault(a => a.ID == id);
                if (admin == null) return NotFound();
                ViewBag.Name = admin.Name;
                ViewBag.Email = admin.Email;
            }
            else if (Role == "Professor")
            {
                var professor = _context.Professors
                     .Where(p => p.ID == id)
                     .Select(p => new
                     {
                         p.Name,
                         p.Email,
                         Courses = p.Courses.Select(c => c.Name).ToList()
                     })
                     .FirstOrDefault(); 
                if (professor == null) return NotFound();
                ViewBag.Name = professor.Name;
                ViewBag.Email = professor.Email;
                ViewBag.Courses = professor.Courses;
            }
            else
            {
                ViewBag.Name = "Super Admin";
                ViewBag.Email = "superadmin@college.com";
            }
            return View();
        }   
    }
}
