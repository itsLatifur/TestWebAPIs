using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.DTOs;
using TestWebAPI.Models;

namespace TestWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new CourseDto
                {
                    Code = c.Code,
                    Name = c.Name,
                    Credits = c.Credits
                })
                .ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<CourseDto>> GetCourse(string code)
        {
            var course = await _context.Courses.FindAsync(code);
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = new CourseDto
            {
                Code = course.Code,
                Name = course.Name,
                Credits = course.Credits
            };
            return Ok(courseDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourse(CourseDto courseDto)
        {
            var course = new Course
            {
                Code = courseDto.Code,
                Name = courseDto.Name,
                Credits = courseDto.Credits
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { code = course.Code }, courseDto);
        }

        [HttpPut("Update/{code}")]
        public async Task<IActionResult> UpdateCourse(string code, CourseDto courseDto)
        {
            var course = await _context.Courses.FindAsync(code);
            if (course == null)
            {
                return NotFound();
            }
            course.Code = courseDto.Code;
            course.Name = courseDto.Name;
            course.Credits = courseDto.Credits;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Delete/{code}")]
        public async Task<IActionResult> DeleteCourse(string code)
        {
            var course = await _context.Courses.FindAsync(code);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
