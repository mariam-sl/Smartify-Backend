using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Lesson
{
    public class LessonListItemDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int OrderNumber { get; set; }

        public bool IsLocked { get;set; }//used in authenticated endpoint
    }
}
