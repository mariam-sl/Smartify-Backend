using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class Lesson 
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public Quiz? Quiz { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public int OrderNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        
        public ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();
    }
}
