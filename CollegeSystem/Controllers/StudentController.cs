using Microsoft.AspNetCore.Mvc;
using CollegeSystem.Data;

namespace CollegeSystem.Controllers
{
    public class StudentController : Controller
    {

        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult MyCourses()
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var studentID = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(studentID))
                return RedirectToAction("Login", "Account");

            int id = int.Parse(studentID);

            var courses = context.Enrollments
                .Where(c => c.StudentID == id)
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
        public IActionResult CollegeCourses()
        {
            //var courses = _context.Courses.ToList();
            return View("CollegeCourses");
        }
        public IActionResult Students()
        {
            return View();
        }
        public IActionResult Professors()
        {
            try
            {
                var professors = _context.Professors.ToList();
                return View(professors);
            }
            catch
            {
                ViewBag.ErrorMessage = "Unable to load professors right now.";
                return View(new List<CollegeSystem.Models.Professor>());
            }
        }
    }
}
 
