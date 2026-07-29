using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Dashboard
{
    public class AdminDashboardDTO
    {
        public int TotalUsers { get; set; }

        public int StudentsCount { get; set; }

        public int InstructorsCount { get; set; }

        public int TotalCourses { get; set; }
        public int PublishedCourses { get; set; }
        public int DraftCourses { get; set; }

        public int TotalEnrollments { get; set; }
    }
}
