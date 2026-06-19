using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class ProfessorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyCourses()
        {
            return View("MyCourses");
        }

        public IActionResult Students()
        {
            return View("Students");
        }

        public IActionResult Professors()
        {
            return View("Professors");
        }

        public IActionResult CollegeCourses()
        {
            return View("CollegeCourses");
        }
    }
}
