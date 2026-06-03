
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class Quiz 
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public string Title { get; set; }
        public int PassingScore { get; set; }
        public int TimeLimit { get; set; }
        public bool IsRetakeAllowed { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
