using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Lesson
{
    public class LessonDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;

        public int OrderNumber { get; set; }
    }
}
