using Smartify.Application.DTO.Enrollment;
using Smartify.Application.DTO.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IEnrollmentService
    {

        Task EnrollAsync(int courseId, string userId);
        Task<IEnumerable<EnrollmentDTO>> GetUserEnrollmentsAsync(string userId);

        Task<CourseProgressDTO?> GetCourseProgressAsync(int courseId, string userId);

        Task<LessonListItemDTO?> GetContinueLessonAsync(int courseId, string userId);
    }
}
