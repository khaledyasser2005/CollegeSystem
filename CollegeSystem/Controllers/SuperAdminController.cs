using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
