using CollegeSystem.Data;
using CollegeSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Security.Claims;

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

            ViewBag.Materials = context.Materials
                .Where(m => m.CourseID == id)
                .OrderByDescending(m => m.UploadDate)
                .ToList();

            var course = context.Courses.FirstOrDefault(c => c.ID == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        public IActionResult Professors()
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var professors = context.Professors.ToList();

            return View("Professors", professors);
        }

        public IActionResult CollegeCourses()
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();
            var courses = context.Courses.ToList();
            return View("CollegeCourses", courses);
        }

        [HttpPost]
        public IActionResult UploadMaterial(int courseId, IFormFile pdfFile, string title, string description)
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            if (pdfFile != null && pdfFile.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "materials");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    pdfFile.CopyTo(stream);
                }

                Material material = new Material();

                material.Title = title;
                material.Description = description;
                material.FileUrl = "/materials/" + fileName;
                material.Type = "PDF";
                material.UploadDate = DateTime.Now;
                material.CourseID = courseId;

                context.Materials.Add(material);

                context.SaveChanges();
            }

            return RedirectToAction("CourseDetails", new { id = courseId });
        }

        public IActionResult DeleteMaterial(int id, int courseId)
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            var material = context.Materials.FirstOrDefault(m => m.ID == id);

            if (material != null)
            {
                string fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    material.FileUrl.TrimStart('/').Replace("/", "\\"));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                context.Materials.Remove(material);

                context.SaveChanges();
            }

            return RedirectToAction("CourseDetails", new { id = courseId });
        }


        public IActionResult UploadReport(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadReport(int courseId, IFormFile file, string title)
        {
            var context = HttpContext.RequestServices.GetService<AppDbContext>();

            if (file == null || file.Length == 0)
            {
                return RedirectToAction("CourseDetails", new { id = courseId });
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);

                var report = new Report
                {
                    Title = title,

                    Type = "PDF",

                    Status = "Pending",

                    GeneratedDate = DateTime.Now,

                    GeneratedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),

                    AdminID = 1, // مؤقتًا لحد ما نربطها بالـ admin login

                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = ms.ToArray()
                };

                context.Reports.Add(report);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("CourseDetails", new { id = courseId });
        }
    }

}