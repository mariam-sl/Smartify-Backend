using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Course
{
    public class CreateCourseDTO
    {
        [Required]
        [StringLength(150, MinimumLength =3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(300, MinimumLength =10)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(5000, MinimumLength =20)]
        public string LongDescription { get; set; } = string.Empty;

        [Required]
        public CourseCategory Category { get; set; }

        [Required]
        public DifficultyLevel Difficulty { get; set; }

        [StringLength(500)]
        public string Thumbnail { get; set; } = string.Empty;

       
    }
}
