using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Enrollment
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int Progress { get; set; }

        public EnrollmentStatus Status { get; set; }

        public DateTime EnrolledAt { get; set; }

        public DateTime? CompletedAt { get; set; }


    }
}
