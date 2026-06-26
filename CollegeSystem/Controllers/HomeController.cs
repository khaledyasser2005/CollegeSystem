using CollegeSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses
        .Include(c => c.Professor)
        .ToList();

            var professors = _context.Professors.ToList();

            ViewBag.Courses = courses;
            ViewBag.Professors = professors;

            return View();
        }
    }
}
