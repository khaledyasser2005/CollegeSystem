using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}