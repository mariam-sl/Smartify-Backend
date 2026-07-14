using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IRepository
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetPublishedAsync();
        Task<IEnumerable<Course>> GetByInstructorAsync(string instructorId);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetDetailedByIdAsync(int id);

        //Task<IEnumerable<Course>> GetByIdsAsync(IEnumerable<int> ids);
        Task AddAsync(Course course);
        //Task<bool> ExistsAsync(int id);
        void Update(Course course);
        void Delete(Course course);
        //Task<IEnumerable<Course>> GetFeaturedAsync();
        //ask<IEnumerable<Course>> GetByAuthorAsync(string authorId);
        //Task<IEnumerable<Course>> SearchAsync(string searchTerm);

        Task SaveChangesAsync();

    }
}
