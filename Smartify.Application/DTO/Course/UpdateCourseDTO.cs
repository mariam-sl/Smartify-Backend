using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Course
{
    public class UpdateCourseDTO
    {
        [StringLength(150, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? ShortDescription { get; set; }

        [StringLength(5000, MinimumLength = 20)]
        public string? LongDescription { get; set; }

        public CourseCategory? Category { get; set; }
        public DifficultyLevel? Difficulty { get; set; }

        [StringLength(500)]
        public string? Thumbnail { get; set; }
    }
}
