using CollegeSystem.Data;
using CollegeSystem.Models;
using CollegeSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        // ================= ADD PROFESSOR =================
        [HttpPost]
        public IActionResult AddProfessor(AddProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageProfessors", model);
            }

            var _exists = _context.Professors.Any(p => p.Email == model.Email);
            if (_exists)
            {
                ModelState.AddModelError("", "Email Already Exists!");
                return View("ManageProfessors", model);
            }

            Professor professor = new Professor();
            professor.Email = model.Email;
            professor.Name = model.Name;

            var hasher = new PasswordHasher<Professor>();
            professor.Password = hasher.HashPassword(professor, model.Password);

            _context.Professors.Add(professor);
            _context.SaveChanges();

            return RedirectToAction("AdminDashboard");
        }

        // ================= UPDATE PROFESSOR =================
        [HttpPost]
        public IActionResult UpdateProfessor(UpdateProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageProfessors", model);
            }

            var professor = _context.Professors.Find(model.ID);

            if (professor == null)
            {
                ModelState.AddModelError("", "Professor not found!");
                return View("ManageProfessors", model);
            }

            if (!String.IsNullOrEmpty(model.Email))
                professor.Email = model.Email;

            if (!String.IsNullOrEmpty(model.Name))
                professor.Name = model.Name;

            if (!String.IsNullOrEmpty(model.Password))
            {
                var hasher = new PasswordHasher<Professor>();
                professor.Password = hasher.HashPassword(professor, model.Password);
            }

            _context.SaveChanges();
            return RedirectToAction("AdminDashboard");
        }

        // ================= DELETE PROFESSOR =================
        [HttpPost]
        public IActionResult DeleteProfessor(DeleteProfessorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageProfessors", model);
            }

            var professor = _context.Professors.Find(model.ID);

            if (professor == null)
            {
                ModelState.AddModelError("", "professor not found!");
                return View("ManageProfessors", model);
            }

            _context.Professors.Remove(professor);
            _context.SaveChanges();

            return RedirectToAction("ManageProfessors");
        }

        // ================= ADD COURSE =================
        [HttpPost]
        public IActionResult AddCourse(AddCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data!");
                return View("ManageCourses", model);
            }

            Course course = new Course();
            course.Name = model.Name;
            course.Description = model.Description;
            course.CoursePrerequisites = model.CoursePrerequisites;
            course.Duration = model.Duration;
            course.ProfessorID = model.ProfessorID;

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("ManageCourses");
        }

        // ================= UPDATE COURSE =================
        [HttpPost]
        public IActionResult UpdateCourse(UpdateCourseViewModel model)
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
                return View("AdminDashboard");
            }

            if (!string.IsNullOrEmpty(model.Name))
                course.Name = model.Name;

            if (!string.IsNullOrEmpty(model.Description))
                course.Description = model.Description;

            if (!string.IsNullOrEmpty(model.CoursePrerequisites))
                course.CoursePrerequisites = model.CoursePrerequisites;

            if (model.Duration.HasValue && model.Duration > 0)
                course.Duration = model.Duration.Value;

            if (model.ProfessorID.HasValue && model.ProfessorID > 0)
                course.ProfessorID = model.ProfessorID.Value;

            _context.SaveChanges();

            return RedirectToAction("ManageCourses");
        }

        // ================= DELETE COURSE =================
        [HttpPost]
        public IActionResult DeleteCourse(DeleteCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageCourses", model);
            }

            var course = _context.Courses.Find(model.ID);

            if (course == null)
            {
                ModelState.AddModelError("", "Course not found!");
                return View("ManageCourses", model);
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("ManageCourses");
        }

        public IActionResult ManageCourses()
        {
            return View("ManageCourses");
        }

        public IActionResult ManageStudents()
        {
            return View("ManageStudents");
        }

        // ================= ADD STUDENT =================
        [HttpPost]
        public IActionResult AddStudent(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View("ManageStudents");
            }

            var _exists = _context.Students.Any(s => s.Email == model.Email);
            if (_exists)
            {
                ModelState.AddModelError("", "Email Already Exists!");
                return View("ManageStudents");
            }

            Student student = new Student();
            student.Email = model.Email;
            student.Name = model.Name;
            student.Level = model.Level;
            student.Phone = model.Phone;
            student.DepartmentID = model.DepartmentID;

            var hasher = new PasswordHasher<Student>();
            student.Password = hasher.HashPassword(student, model.Password);

            _context.Students.Add(student);
            _context.SaveChanges();

            return RedirectToAction("AdminDashboard");
        }

        // ================= UPDATE STUDENT =================
        [HttpPost]
        public IActionResult UpdateStudent(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View("ManageStudents");
            }

            var student = _context.Students.Find(model.ID);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found!");
                return View("ManageStudents");
            }

            if (!String.IsNullOrEmpty(model.Name))
                student.Name = model.Name;

            if (!String.IsNullOrEmpty(model.Email))
                student.Email = model.Email;

            if (!String.IsNullOrEmpty(model.Phone))
                student.Phone = model.Phone;

            student.DepartmentID = model.DepartmentID;
            student.Level = model.Level;

            if (!String.IsNullOrEmpty(model.Password))
            {
                var hasher = new PasswordHasher<Student>();
                student.Password = hasher.HashPassword(student, model.Password);
            }

            _context.SaveChanges();
            return RedirectToAction("ManageStudents");
        }

        // ================= DELETE STUDENT =================
        [HttpPost]
        public IActionResult DeleteStudent(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
                return View("ManageStudents");
            }

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

       
        public IActionResult ManageDepartments()
        {
            return View("ManageDepartments");
        }
    }
}
