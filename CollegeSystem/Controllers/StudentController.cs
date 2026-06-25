using Microsoft.AspNetCore.Mvc;
using CollegeSystem.Data;

namespace CollegeSystem.Controllers
{
    public class StudentController : Controller
    {

        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult MyCourses()
        {
            return View();
        }
        public IActionResult CollegeCourses()
        {
            var courses = _context.Courses.ToList();

            return View(courses);
        }
        public IActionResult Students()
        {
            return View();
        }
        public IActionResult Professors()
        {
            try
            {
                var professors = _context.Professors.ToList();
                return View(professors);
            }
            catch
            {
                ViewBag.ErrorMessage = "Unable to load professors right now.";
                return View(new List<CollegeSystem.Models.Professor>());
            }
        }
    }
}
 
