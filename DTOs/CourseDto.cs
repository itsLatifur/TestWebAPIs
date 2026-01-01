using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs
{
    public class CourseDto
    {
        [Required(ErrorMessage = "Course code is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Course code must be 2-10 characters")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Course name must be 3-200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credits are required")]
        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        public int Credits { get; set; }
    }
}
