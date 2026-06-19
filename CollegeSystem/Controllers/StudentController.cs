using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult MyCourses()
        {
            return View();
        }
        public IActionResult CollegeCourses()
        {
            return View();
        }
        public IActionResult Students()
        {
            return View();
        }
        public IActionResult Professors()
        {
            return View();
        }
    }
}
