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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment?> GetByUserAndCourseAsync(string userId,int courseId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Enrollment>> GetByUserAsync(string userId)
        {
            return await _context.Enrollments
                 .Include(e => e.Course)
                .Where(e => e.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public void Update (Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        public void Delete (Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }









    }
}
