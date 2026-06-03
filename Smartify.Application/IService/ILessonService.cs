using Smartify.Application.DTO.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonListItemDTO>> GetCourseLessonsAsync(int courseId);
        Task<LessonDetailsDTO?> GetLessonDetailsAsync(int lessonId, string userId, bool isAdmin, bool isInstructor);

        Task<LessonDTO?> CreateLessonAsync(CreateLessonDTO lessonDTO,string userId,bool isAdmin);
        Task<LessonDTO?> UpdateLessonAsync(int LessonId,UpdateLessonDTO lessonDTO,string userId,bool isAdmin);
        Task DeleteLessonAsync(int lessonId,string userId,bool isAdmin);

        Task MarkLessonCompletedAsync(int lessonId, string userId);

        Task<IEnumerable<LessonListItemDTO>> GetCourseLessonsWithProgressAsync(int courseId, string userId,bool isAdmin,bool isInstructor);
    }
}
