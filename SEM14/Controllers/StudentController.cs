
using Microsoft.AspNetCore.Mvc;
using SEM14.Models;

namespace SEM14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Search")]
        public IActionResult SearchStudents(string? name, string? lastName, string? email)
        {
            var response = _context.Students
                .Where(s => s.IsActive &&
                    (name == null || s.FirstName.Contains(name)) &&
                    (lastName == null || s.LastName.Contains(lastName)) &&
                    (email == null || s.Email.Contains(email)))
                .OrderByDescending(s => s.LastName)
                .ToList();

            return Ok(response);
        }

        [HttpGet("SearchByNameAndGrade")]
        public IActionResult SearchByNameAndGrade(string? name, string? grade)
        {
            var response = _context.Students
                .Where(s => s.IsActive &&
                    (name == null || s.FirstName.Contains(name)) &&
                    (grade == null || s.Grade.Name.Contains(grade)))
                .OrderByDescending(s => s.Enrollments
                    .Where(e => e.IsActive)
                    .Select(e => e.Course.Name)
                    .FirstOrDefault())
                .Select(s => new
                {
                    Student = new
                    {
                        s.Id,
                        s.FirstName,
                        s.LastName,
                        s.Phone,
                        s.Email,
                        s.GradeId
                    },
                    Grade = new
                    {
                        s.Grade.Id,
                        s.Grade.Name,
                        s.Grade.Description
                    }
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("SearchEnrollmentsByCourse")]
        public IActionResult SearchEnrollmentsByCourse(string? course)
        {
            var response = _context.Enrollments
                .Where(e => e.IsActive && e.Student.IsActive &&
                    (course == null || e.Course.Name.Contains(course)))
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .Select(e => new
                {
                    Student = new
                    {
                        e.Student.Id,
                        e.Student.FirstName,
                        e.Student.LastName,
                        e.Student.Phone,
                        e.Student.Email
                    },
                    Course = new
                    {
                        e.Course.Id,
                        e.Course.Name,
                        e.Course.Credit
                    },
                    Enrollment = new
                    {
                        e.Id,
                        e.EnrollmentDate,
                        e.IsActive
                    }
                })
                .ToList();

            return Ok(response);
        }


        [HttpGet("SearchEnrollmentsByGrade")]
        public IActionResult SearchEnrollmentsByGrade(string? grade)
        {
            var response = _context.Enrollments
                .Where(e => e.IsActive &&
                            e.Student.IsActive &&
                            e.Student.Grade.IsActive &&
                            (grade == null ||
                             e.Student.Grade.Name.Contains(grade)))
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .Select(e => new
                {
                    Student = new
                    {
                        e.Student.Id,
                        e.Student.FirstName,
                        e.Student.LastName,
                        e.Student.Phone,
                        e.Student.Email,
                        e.Student.GradeId
                    },
                    Grade = new
                    {
                        e.Student.Grade.Id,
                        e.Student.Grade.Name,
                        e.Student.Grade.Description
                    },
                    Course = new
                    {
                        e.Course.Id,
                        e.Course.Name,
                        e.Course.Credit
                    },
                    Enrollment = new
                    {
                        e.Id,
                        e.EnrollmentDate,
                        e.IsActive
                    }
                })
                .ToList();

            return Ok(response);
        }

    }
}
