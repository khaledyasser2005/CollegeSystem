using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return PartialView();
        }

        public IActionResult AdminDashboard()
        {
            return PartialView();
        }

        public IActionResult CollegeCourses()
        {
            return PartialView();
        }

        public IActionResult AssignCourseToStudent()
        {
            return PartialView();
        }

        public IActionResult AssignCourseToProfessor()
        {
            return PartialView();
        }
    }
}