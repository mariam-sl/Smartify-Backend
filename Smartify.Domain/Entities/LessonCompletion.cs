using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class LessonCompletion 
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public string UserId { get; set; } // Identity reference
     
        public DateTime CompletedAt { get; set; }
    }
}
