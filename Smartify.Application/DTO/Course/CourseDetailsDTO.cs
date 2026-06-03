using Smartify.Application.DTO.Lesson;
using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Course
{
    public class CourseDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public CourseCategory Category { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }

        public List<LessonListItemDTO> Lessons { get; set; } = new();

    }
}
