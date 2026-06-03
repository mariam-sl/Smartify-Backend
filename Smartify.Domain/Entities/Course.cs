using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public CourseCategory Category { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public string Thumbnail { get; set; }
        public string CreatedById { get; set; } // Identity UserId
        public DateTime CreatedAt { get; set; }
        
        public bool IsPublished { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public ICollection<Quiz> Quizzes { get; set; }= new List<Quiz>();

       public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();


    }
}
