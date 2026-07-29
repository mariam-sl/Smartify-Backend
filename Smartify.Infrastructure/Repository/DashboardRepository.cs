using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartify.Application.DTO.Dashboard;
using Smartify.Application.IRepository;
using Smartify.Domain.Constants;
using Smartify.Infrastructure.Data;
using Smartify.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Repository
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardRepository(AppDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardDTO> GetDashboardSummaryAsync()
        {
            var students =await _userManager.GetUsersInRoleAsync(Roles.Student);

            var instructors =await _userManager.GetUsersInRoleAsync(Roles.Instructor);

            return new AdminDashboardDTO
            {
                TotalUsers = await _context.Users.CountAsync(),

                StudentsCount = students.Count,

                InstructorsCount = instructors.Count,

                TotalCourses = await _context.Courses.CountAsync(),

                PublishedCourses = await _context.Courses
                   .CountAsync(c => c.IsPublished),

                DraftCourses = await _context.Courses
                   .CountAsync(c => !c.IsPublished),

                TotalEnrollments = await _context.Enrollments.CountAsync()
            };
        }

    }
}
