using Microsoft.AspNetCore.Identity;
using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 

       
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        // Navigation for courses created by this user
        public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        
    }
}