using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.DTO.Certificate
{
    public class CreateCertificateDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = default!;

        [Required]
        [StringLength(500)]
        public string ShortDescription { get; set; } = default!;

        public string? LongDescription { get; set; }

        public CourseCategory? Category { get; set; }
        public DifficultyLevel? Difficulty { get; set; }
        [StringLength(1000)]
        public string? Thumbnail { get; set; }
        public bool? IsPublished { get; set; }
    }
}
