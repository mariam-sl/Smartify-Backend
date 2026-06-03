using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Lesson
{
    public class UpdateLessonDTO
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        public int OrderNumber { get; set; }
    }
}
