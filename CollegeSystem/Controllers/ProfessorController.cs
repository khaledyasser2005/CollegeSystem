using CollegeSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CollegeSystem.Controllers
{
    public class ProfessorController : Controller
    {
        public IActionResult MyCourses()
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var professorId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(professorId))
                return RedirectToAction("Login", "Account");

            int id = int.Parse(professorId);

            var courses = context.Courses
                .Where(c => c.ProfessorID == id)
                .ToList();

            return View(courses);
        }
        public IActionResult CourseDetails(int id)
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var course = context.Courses.FirstOrDefault(c => c.ID == id);

            if (course == null)
                return NotFound();

            return View(course);
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