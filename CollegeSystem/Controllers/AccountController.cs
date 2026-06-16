using CollegeSystem.Data;
using CollegeSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                var student = _context.Students.FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);
                if (student != null)
                {
                    HttpContext.Session.SetString("UserId",student.ID.ToString());
                    HttpContext.Session.SetString("Role", "Student");
                    return RedirectToAction("Index", "Student");
                }
            }
            else if(model.Role == "Professor")
            {
                var professor = _context.Professors.FirstOrDefault(p => p.Email == model.Email && p.Password == model.Password);
                if (professor != null)
                {
                    HttpContext.Session.SetString("UserId", professor.ID.ToString());
                    HttpContext.Session.SetString("Role", "Professor");
                    return RedirectToAction("Index", "Professor");
                }
            }
            else if(model.Role == "Admin")
            {
                var Admin = _context.Admins.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
                if (Admin != null)
                {
                    HttpContext.Session.SetString("UserId", Admin.ID.ToString());
                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToAction("Index", "Admin");
                }
            }
            else if(model.Role == "SuperAdmin")
            {
                var SuperAdmin = _context.SuperAdmins.FirstOrDefault(S => S.Email == model.Email && S.Password == model.Password);
                if (SuperAdmin != null)
                {
                    HttpContext.Session.SetString("UserId", SuperAdmin.ID.ToString());
                    HttpContext.Session.SetString("Role", "SuperAdmin");
                    return RedirectToAction("Index", "SuperAdmin");
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
