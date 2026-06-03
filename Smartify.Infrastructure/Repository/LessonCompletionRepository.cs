using Microsoft.EntityFrameworkCore;
using Smartify.Application.IRepository;
using Smartify.Domain.Entities;
using Smartify.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Repository
{
    public class LessonCompletionRepository : ILessonCompletionRepository
    {
        private readonly AppDbContext _context;

        public LessonCompletionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LessonCompletion?> GetAsync(string userId,int lessonId)
        {
            return await _context.LessonCompletions
               .FirstOrDefaultAsync(lc =>
                   lc.UserId == userId &&
                   lc.LessonId == lessonId);
        }

        public async Task<int> GetCompletedCount(string userId,int courseId)
        {
            return await _context.LessonCompletions
                .CountAsync(lc =>
                lc.UserId == userId &&
                lc.Lesson.CourseId == courseId);
        }

        public async Task AddAsync(LessonCompletion completion)
        {
            await _context.LessonCompletions.AddAsync(completion);
        }

        public async Task<List<int>> GetCompletedLessonIds(string userId, int courseId)
        {
            return await _context.LessonCompletions
                .Where(lc => lc.UserId == userId && lc.Lesson.CourseId == courseId)
                .Select(lc => lc.LessonId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
