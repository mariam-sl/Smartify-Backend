using Microsoft.EntityFrameworkCore;
using Smartify.Application.DTO.Course;
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

        public async Task<IEnumerable<Course>> GetPublishedAsync()
        {
            return await _context.Courses
                .Where(c => c.IsPublished)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetByInstructorAsync(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.CreatedById == instructorId)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(CourseQueryParameters parameters,string? instructorId = null)
        {
            IQueryable<Course> query = _context.Courses.AsNoTracking();

            //instructor filter
            if (!string.IsNullOrWhiteSpace(instructorId))
            {
                query = query.Where(c =>
                    c.CreatedById == instructorId);
            }


            //search
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(c =>
                    c.Title.Contains(parameters.Search) ||
                    c.ShortDescription.Contains(parameters.Search));
            }

            //category
            if (parameters.Category.HasValue)
            {
                query = query.Where(c =>
                    c.Category == parameters.Category.Value);
            }

            //difficulty
            if (parameters.Difficulty.HasValue)
            {
                query = query.Where(c =>
                    c.Difficulty == parameters.Difficulty.Value);
            }


            //publish status
            if (parameters.IsPublished.HasValue)
            {
                query = query.Where(c =>
                    c.IsPublished == parameters.IsPublished.Value);
            }

            //sorting
            bool descending = parameters.SortOrder?.ToLower() == "desc";

            query = parameters.SortBy?.ToLower() switch
            {
                "title" =>
                    descending
                        ? query.OrderByDescending(c => c.Title)
                        : query.OrderBy(c => c.Title),

                "difficulty" =>
                    descending
                        ? query.OrderByDescending(c => c.Difficulty)
                        : query.OrderBy(c => c.Difficulty),

                _ =>
                    descending
                        ? query.OrderByDescending(c => c.CreatedAt)
                        : query.OrderBy(c => c.CreatedAt)
            };

            return await query.ToListAsync();


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
