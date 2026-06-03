using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IRepository
{
    public interface ILessonCompletionRepository
    {
        Task<LessonCompletion?> GetAsync(string userId,int lessonId);

        Task AddAsync(LessonCompletion completion);

        Task<int> GetCompletedCount(string userId, int courseId);

        Task<List<int>> GetCompletedLessonIds(string userId, int courseId);

        Task SaveChangesAsync();
    }
}
