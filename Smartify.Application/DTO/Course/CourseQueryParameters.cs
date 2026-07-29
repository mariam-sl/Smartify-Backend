using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Course
{
    public  class CourseQueryParameters
    {
        public string? Search { get; set; }

        public DifficultyLevel? Difficulty { get; set; }

        public CourseCategory? Category { get; set; }

        public bool? IsPublished { get; set; }

         // title | createdAt | difficulty
         public string? SortBy { get; set; }

         // asc | desc
        public string? SortOrder { get; set; } = "asc";
    }
}
