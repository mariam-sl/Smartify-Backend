using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Course
{
    public class CourseListItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public DifficultyLevel Difficulty { get; set; }
        public CourseCategory Category { get; set; }
        public string Thumbnail { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

    }
}
