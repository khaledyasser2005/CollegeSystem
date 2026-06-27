using CollegeSystem.Data;
using CollegeSystem.Models;
using CollegeSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CollegeSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        //Login --Get
        public IActionResult Login()
        {
            return View();
        }

        //Login --Post
        [HttpPost] 
        public IActionResult Login(LoginViewModel model)
        {
            if(model.Role == "Student")
            {
                var student = _context.Students.FirstOrDefault(s => s.Email == model.Email);
                if (student != null)
                {
                    var hasher = new PasswordHasher<Student>();
                    var result = hasher.VerifyHashedPassword(student, student.Password, model.Password);

                    if(result == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId",student.ID.ToString());
                        HttpContext.Session.SetString("Role", "Student");
                        return RedirectToAction("MyCourses", "Student");
                    }
                }
            }
            else if(model.Role == "Professor")
            {
                var professor = _context.Professors.FirstOrDefault(p => p.Email == model.Email);
                if (professor != null)
                {
                    var hasher = new PasswordHasher<Professor>();
                    var result = hasher.VerifyHashedPassword(professor, professor.Password, model.Password);

                    if(result == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", professor.ID.ToString());
                        HttpContext.Session.SetString("Role", "Professor");
                        return RedirectToAction("MyCourses", "Professor");
                    }
                }
            }
            else if(model.Role == "Admin")
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Email == model.Email);
                if (admin != null)
                {
                    var hasher = new PasswordHasher<Admin>();
                    var result = hasher.VerifyHashedPassword(admin, admin.Password, model.Password);

                    if(result == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", admin.ID.ToString());
                        HttpContext.Session.SetString("Role", "Admin");
                        return RedirectToAction("AdminDashboard", "Admin");       
                    }
                }
            }
            else if(model.Role == "SuperAdmin")
            {
                var superAdmin = _context.SuperAdmins.FirstOrDefault(S => S.Email == model.Email);
                if (superAdmin != null)
                {
                    var hasher = new PasswordHasher<SuperAdmin>();
                    var result = hasher.VerifyHashedPassword(superAdmin, superAdmin.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", superAdmin.ID.ToString());
                        HttpContext.Session.SetString("Role", "Super Admin");
                        return RedirectToAction("SuperAdminDashboard", "SuperAdmin");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Email or Password");
            return View(model);
        }

        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}