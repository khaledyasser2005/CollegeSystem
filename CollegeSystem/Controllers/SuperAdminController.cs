using CollegeSystem.Data;
using CollegeSystem.Models;
using CollegeSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.ComponentModel.DataAnnotations;


namespace CollegeSystem.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly AppDbContext _context;
        public SuperAdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult SuperAdminDashboard()
        {
            return View("SuperAdminDashboard");
        }

        [HttpPost]
        public IActionResult AddAdmin(AdminModel model)
        {
            if(!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }

            var _exists = _context.Admins.Any(x=> x.Email == model.Email);
            if(_exists)
            {
                ModelState.AddModelError("", "Email Already Exists!");
                return View(model);
            }

            Admin admin = new Admin();
            admin.Email = model.Email;
            admin.Name = model.Name;

            var hasher = new PasswordHasher<Admin>();
            admin.Password = hasher.HashPassword(admin, model.Password);

            _context.Admins.Add(admin);
            _context.SaveChanges();
            return RedirectToAction("SuperAdminDashboard");
        }

        // ================= UPDATE ADMIN =================
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public IActionResult UpdateAdmin(int id, AdminModel model)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.ID == id);

            if (admin == null)
                return NotFound();

            admin.Name = model.Name;
            admin.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var hasher = new PasswordHasher<Admin>();
                admin.Password = hasher.HashPassword(admin, model.Password);
            }

            _context.SaveChanges();

            return RedirectToAction("SuperAdminDashboard");
        }

        // ================= DELETE ADMIN =================
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public IActionResult DeleteAdmin(int id)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.ID == id);

            if (admin == null)
                return NotFound();

            _context.Admins.Remove(admin);
            _context.SaveChanges();

            return RedirectToAction("SuperAdminDashboard");
        }
    }
}
