using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEM14.DTO.Request;
using SEM14.DTO.Response;
using SEM14.Models;

namespace SEM14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<GradeResponseV1> GetGrade()
        {
            var grades = _context.Grades
                .Where(g => g.IsActive == true)
                .ToList();

            var response = grades.Select(g => new GradeResponseV1
            {
                IdGrade = g.Id,
                Name = g.Name,
                Description = g.Description
            }).ToList();

            return response;
        }

        [HttpGet("{id}")]
        public IActionResult GetGradeById(int id)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.Id == id && g.IsActive == true);

            if (grade == null)
            {
                return NotFound("Grade not found");
            }

            var response = new GradeResponseV1
            {
                IdGrade = grade.Id,
                Name = grade.Name,
                Description = grade.Description
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateGrade(GradeRequestV1 request)
        {
            var grade = new Grade
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true
            };

            _context.Grades.Add(grade);
            _context.SaveChanges();

            return Ok("Grade created successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGrade(int id, GradeRequestV1 request)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.Id == id && g.IsActive == true);

            if (grade == null)
            {
                return NotFound("Grade not found");
            }

            grade.Name = request.Name;
            grade.Description = request.Description;

            _context.Grades.Update(grade);
            _context.SaveChanges();

            return Ok("Grade updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGrade(int id)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.Id == id && g.IsActive == true);

            if (grade == null)
            {
                return NotFound("Grade not found");
            }

            grade.IsActive = false;

            _context.Grades.Update(grade);
            _context.SaveChanges();

            return Ok("Grade deleted successfully");
        }
    }
}
