using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public  class Enrollment 
    {
        public int Id { get; set; }
        public string UserId { get; set; }//Identity reference

        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        public int Progress { get; set; }
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
