using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IRepository
{
    public interface IEnrollmentRepository
    {

        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId);
        Task<IEnumerable<Enrollment>> GetByUserAsync(string userId);
        Task AddAsync(Enrollment enrollment);
        void Update(Enrollment enrollment);
        void Delete(Enrollment enrollment);
        Task SaveChangesAsync();
    }
}
