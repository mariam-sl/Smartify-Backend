using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Enrollment
{
    public  class CourseProgressDTO
    {
        public int CourseId { get; set; }

        public int Progress { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int CompletedLessons { get; set; }

        public int TotalLessons { get; set; }
    }
}
