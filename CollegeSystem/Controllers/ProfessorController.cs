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

            var professor = context.Professors.FirstOrDefault();

            if (professor == null)
                return NotFound();

            var courses = context.Courses
                .Where(c => c.ProfessorID == professor.ID)
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
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var professors = context.Professors.ToList();

            return View("Professors", professors);
        }

        public IActionResult CollegeCourses()
        {
            return View("CollegeCourses");
        }
    }
}