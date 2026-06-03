using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IRepository
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId);
        Task<Lesson?> GetByIdAsync(int LessonId);

        Task AddAsync(Lesson lesson);
        Task<int> GetLessonsCountAsync(int courseId);
        void Update(Lesson lesson);
        void Delete(Lesson lesson);
        Task SaveChangesAsync();

    }
}
