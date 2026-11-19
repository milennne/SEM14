using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEM14.DTO.Request;
using SEM14.DTO.Response;
using SEM14.Models;

namespace SEM14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<CourseResponseV1> GetCourse()
        {
            var courses = _context.Courses
                .Where(c => c.IsActive == true)
                .ToList();

            var response = courses.Select(c => new CourseResponseV1
            {
                IdCourse = c.Id,
                Name = c.Name,
                Credit = c.Credit
            }).ToList();

            return response;
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.Id == id && c.IsActive == true);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            var response = new CourseResponseV1
            {
                IdCourse = course.Id,
                Name = course.Name,
                Credit = course.Credit
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateCourse(CourseRequestV1 request)
        {
            var course = new Course
            {
                Name = request.Name,
                Credit = request.Credit,
                IsActive = true
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return Ok("Course created successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, CourseRequestV1 request)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.Id == id && c.IsActive == true);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            course.Name = request.Name;
            course.Credit = request.Credit;

            _context.Courses.Update(course);
            _context.SaveChanges();

            return Ok("Course updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.Id == id && c.IsActive == true);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            course.IsActive = false;

            _context.Courses.Update(course);
            _context.SaveChanges();

            return Ok("Course deleted successfully");
        }
    }
}
