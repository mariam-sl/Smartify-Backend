using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class Certificate
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; } // Identity reference
        
        
        public string DownloadUrl { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
