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
        public IActionResult AllProfessors(string searchName)
        {
            var professors = _context.Professors.AsQueryable();

            if (!String.IsNullOrEmpty(searchName))
            {
                professors = professors.Where(p => p.Name.Contains(searchName));
            }

            ViewBag.SearchName = searchName;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProfessorsTablePartial", professors.ToList());
            }

            return View("AllProfessors", professors.ToList());
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
            course.ProfessorID = (model.ProfessorID.HasValue && model.ProfessorID.Value > 0)
                  ? model.ProfessorID
                  : null;

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
        public IActionResult AddStudent(AddStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
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

            return RedirectToAction("ManageStudents");
        }

        // ================= UPDATE STUDENT =================
        [HttpPost]
        public IActionResult UpdateStudent(UpdateStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageStudents", model);
            }

            var student = _context.Students.Find(model.ID);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found!");
                return View("ManageStudents", model);
            }

            if (!String.IsNullOrEmpty(model.Name))
                student.Name = model.Name;

            if (!String.IsNullOrEmpty(model.Email))
                student.Email = model.Email;

            if (!String.IsNullOrEmpty(model.Phone))
                student.Phone = model.Phone;

            if (model.DepartmentID.HasValue)
                student.DepartmentID = model.DepartmentID.Value;

            if (model.Level.HasValue)
                student.Level = model.Level.Value;

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
        public IActionResult DeleteStudent(DeleteStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageStudents", model);
            }

            var student = _context.Students.Find(model.ID);

            if (student == null)
            {
                ModelState.AddModelError("", "Student not found!");
                return View("ManageStudents", model);
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction("ManageStudents");
        }

        public IActionResult CollegeCourses()
        {
            var courses = _context.Courses.Include(c=>c.Professor).ToList();
            return View("CollegeCourses", courses);
        }

        public IActionResult ManageDepartments()
        {
            return View("ManageDepartments");
        }

        //Add Department
        [HttpPost]
        public IActionResult AddDepartment(AddDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageDepartments", model);
            }
            var _exists = _context.Departments.Any(s => s.Name == model.Name);
            if (_exists)
            {
                ModelState.AddModelError("", "Department Already Exists!");
                return View("ManageDepartments", model);
            }
            Department department = new Department();
            if (!String.IsNullOrEmpty(model.Name))
            {
                department.Name = model.Name;
            }
            if (model.GPARequired > 0.00)
            {
                department.GPARequired = model.GPARequired;
            }
            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction("ManageDepartments");
        }

        //Update Department
        [HttpPost]
        public IActionResult UpdateDepartment(UpdateDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageDepartments", model);
            }

            var department = _context.Departments.Find(model.ID);

            if (department == null)
            {
                ModelState.AddModelError("", "Department not found!");
                return View("ManageDepartments", model);
            }

            if (!String.IsNullOrEmpty(model.Name))
                department.Name = model.Name;

            if (model.GPARequired.HasValue)
                department.GPARequired = model.GPARequired.Value;

            _context.SaveChanges();
            return RedirectToAction("ManageDepartments");
        }

        //Delete Department
        [HttpPost]
        public IActionResult DeleteDepartment(DeleteDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Data");
                return View("ManageDepartments", model);
            }

            var department = _context.Departments.Find(model.ID);

            if (department == null)
            {
                ModelState.AddModelError("", "Department not found!");
                return View("ManageDepartments", model);
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();

            return RedirectToAction("ManageDepartments");
        }
        public IActionResult AssignCourseToStudent()
        {
            var students = _context.Students.ToList();
            var courses = _context.Courses.ToList();
            ViewBag.Students = students;
            ViewBag.Courses = courses;
            return View("AssignCourseToStudent");
        }

        [HttpPost]
        public IActionResult AssignCourseToStudent(int StudentID, int CourseID)
        {
            var exists = _context.Enrollments
                .Any(e => e.StudentID == StudentID && e.CourseID == CourseID);

            if (exists)
            {
                ModelState.AddModelError("", "Student is already enrolled in this course!");
                var students = _context.Students.ToList();
                var courses = _context.Courses.ToList();
                ViewBag.Students = students;
                ViewBag.Courses = courses;
                return View("AssignCourseToStudent");
            }

            var enrollment = new Enrollment
            {

                StudentID = StudentID,
                CourseID = CourseID,
                Grade = "N/A",
                Semester = "Current"
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();
            return RedirectToAction("AssignCourseToStudent");
        }
        public IActionResult Reports()
        {
            var reports = _context.Reports.ToList();
            return View(reports);
        }
        public IActionResult DownloadReport(int id)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ID == id);
            if (report == null)
                return NotFound();
            return File(report.Data, report.ContentType, report.FileName);
        }


        public IActionResult StudentsByLevel()
        {
            var students = _context.Students
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToList();

            var grouped = students
                .GroupBy(s => s.Level)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }
        
        public IActionResult CollegeDepartments()
        {
            var departments = _context.Departments.ToList();
            return View(departments);
        }


        // ================= PROFESSORS COURSE ASSIGNMENT =================

        public IActionResult ProfessorsCourseAssignment()
        {
            var courses = _context.Courses
                .Include(c => c.Professor)
                .ToList();

            ViewBag.Professors = _context.Professors.ToList();

            return View("ProfessorsCourseAssignment", courses);
        }

        [HttpPost]
        public IActionResult AssignCourseToProfessor(int ProfessorID, int CourseID)
        {
            var course = _context.Courses.Find(CourseID);

            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("ProfessorsCourseAssignment");
            }

            if (course.ProfessorID == ProfessorID)
            {
                TempData["ErrorMessage"] = "This course is already assigned to the selected professor.";
                return RedirectToAction("ProfessorsCourseAssignment");
            }

            course.ProfessorID = ProfessorID;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course assigned to professor successfully.";
            return RedirectToAction("ProfessorsCourseAssignment");
        }

        [HttpPost]
        public IActionResult UnassignCourseFromProfessor(int CourseID)
        {
            var course = _context.Courses.Find(CourseID);

            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("ProfessorsCourseAssignment");
            }

            course.ProfessorID = null;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course unassigned successfully.";
            return RedirectToAction("ProfessorsCourseAssignment");
        }
    }
}
