using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Course
    {
        [Key]
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
    }
}
