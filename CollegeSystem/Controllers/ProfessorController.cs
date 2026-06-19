using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class ProfessorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
