using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllCourses()
        {
            return View();
        }
        public IActionResult AllProfessors()
        {
            return View();
        }
    }
}