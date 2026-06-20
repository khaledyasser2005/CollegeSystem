using CollegeSystem.Data;
using CollegeSystem.Models;
using CollegeSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CollegeSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }
        public IActionResult ManageProfessors()
        {
            return View("ManageProfessors");
        }
        [HttpPost]
        public IActionResult AddProfessor(ProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View("ManageProfessors");
            }
            var _exists = _context.Professors.Any(p => p.Email == model.Email);
            if(_exists)
            {
                ModelState.AddModelError("", "Email Already Exists!");
                return View("ManageProfessors");
            }
            Professor professor = new Professor();
            professor.Email = model.Email;
            professor.Name = model.Name;

            var hasher = new PasswordHasher<Professor>();
            professor.Password = hasher.HashPassword(professor,model.Password);

            _context.Professors.Add(professor);
            _context.SaveChanges();
            return RedirectToAction("AdminDashboard");
        }
        [HttpPost]
        public IActionResult UpdateProfessor(ProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }
            var professor = _context.Professors.Find(model.ID);

            if(professor == null)
            {
                ModelState.AddModelError("", "Professor not found!");
                return View(model);
            }
            if(!String.IsNullOrEmpty(model.Email)){ 
                professor.Email=model.Email;
            }
            if (!String.IsNullOrEmpty(model.Name)){
                professor.Name = model.Name;
            }
            if (!String.IsNullOrEmpty(model.Password)){
                var hasher = new PasswordHasher<Professor>();
                professor.Password = hasher.HashPassword(professor, model.Password);
            }
            _context.SaveChanges();
            return RedirectToAction("AdminDashboard");

        }
        [HttpPost]
        public IActionResult AddCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data!");
                return View("ManageCourses");
            }

            Course course = new Course();

            course.Name = model.Name;

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("AdminDashboard");
        }
        [HttpPost]
        public IActionResult UpdateCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data!");
                return View(model);
            }

            var course = _context.Courses.Find(model.ID);

            if (course == null)
            {
                ModelState.AddModelError("", "Course not found!");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                course.Name = model.Name;
            }

            _context.SaveChanges();

            return RedirectToAction("AdminDashboard");
        }



        public IActionResult ManageCourses()
        {
            return View("ManageCourses");
        }
        public IActionResult ManageStudents()
        {
            return View("ManageStudents");

        }
        [HttpPost]
        public IActionResult UpdateStudent(StudentViewModel model)
        {
            var student = _context.Students.Find(model.ID);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found!");
                return View("ManageStudents");
            }
            if (!String.IsNullOrEmpty(model.Name))
            {
                student.Name = model.Name;
            }
            if (!String.IsNullOrEmpty(model.Email))
            {
                student.Email = model.Email;
            }
            if (!String.IsNullOrEmpty(model.Password))
            {
                var hasher = new PasswordHasher<Student>();
                student.Password = hasher.HashPassword(student, model.Password);
            }
            _context.SaveChanges();
            return RedirectToAction("ManageStudents");
        }
        [HttpPost]
        public IActionResult DeleteStudent(StudentViewModel model)
        {
            var student = _context.Students.Find(model.ID);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found!");
                return View("ManageStudents");
            }
            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction("ManageStudents");
        }
        public IActionResult CollegeCourses()
        {
            return View("CollegeCourses");
        }

        public IActionResult AssignCourseToStudent()
        {
            return View("AssignCourseToStudent");
        }

        public IActionResult AssignCourseToProfessor()
        {
            return View("AssignCourseToProfessor");
        }
    }
}