using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
