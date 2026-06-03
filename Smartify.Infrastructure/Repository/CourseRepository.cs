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
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.AsNoTracking().ToListAsync();
        }
        public async Task<Course?> GetDetailedByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .Include(c => c.Quizzes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

       /* public async Task<IEnumerable<Course>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Courses
                .Where(c => ids.Contains(c.Id)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetFeaturedAsync()
        {
            return await _context.Courses
                .Where(c => c.IsPublished)
                .OrderByDescending(c => c.CreatedAt)
                .Take(5).AsNoTracking().ToListAsync();
        }
*/
       /* public async Task<IEnumerable<Course>> GetByAuthorAsync(string authorId)
        {
            return await _context.Courses
                .Where(c => c.CreatedById == authorId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> SearchAsync(string searchTerm)
        {
            return await _context.Courses
                .Where(c =>
                    c.Title.Contains(searchTerm) ||
                    c.ShortDescription.Contains(searchTerm))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courses
                .AnyAsync(c => c.Id == id);
        }*/

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        public void Delete(Course course)
        {
            _context.Courses.Remove(course);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
