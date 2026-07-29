using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartify.Application.DTO.Common;
using Smartify.Application.DTO.User;
using Smartify.Application.IService;
using Smartify.Domain.Constants;
using Smartify.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDTO> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty
            };
        }

        public async Task<InstructorDTO> CreateInstructorAsync(CreateInstructorDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if(existingUser!=null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var instructor = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(instructor, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create instructor");
            }

            await _userManager.AddToRoleAsync(instructor, Roles.Instructor);
            return new InstructorDTO
            {
                Id = instructor.Id,
                FullName = instructor.FullName,
                Email = instructor.Email,
                CreatedAt = instructor.CreatedAt
            }
            ;

        }

        public async Task<IEnumerable<InstructorDTO>> GetInstructorsAsync()
        {
            var instructors = await _userManager.GetUsersInRoleAsync(Roles.Instructor);
            return instructors.Select(i =>new InstructorDTO
               {
                   Id = i.Id,
                   FullName = i.FullName,
                   Email = i.Email!,
                   CreatedAt = i.CreatedAt
               });
        }

        public async Task<PagedResult<UserListDTO>> GetUsersAsync(UserQueryParameters parameters)
        {
            var users = _userManager.Users.AsQueryable();

            //search filter
            if(!string.IsNullOrEmpty(parameters.Search))
            {
                users = users.Where( u =>
                            u.FullName.Contains(parameters.Search)
                                ||
                            u.Email!.Contains(parameters.Search));
            }

            var usersList = await users.ToListAsync();
            var result = new List<UserListDTO>();
            foreach(var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                if (!string.IsNullOrEmpty(parameters.Role) && role != parameters.Role)
                {
                    continue;
                }

                result.Add(new UserListDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Role = role ?? "",
                    CreatedAt = user.CreatedAt
                });
            }

            var totalCount = result.Count;


            var pagedUsers = result
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize);



            return new PagedResult<UserListDTO>
            {
                Items = pagedUsers,
                TotalCount = totalCount,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };


        }

    

    public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
                throw new KeyNotFoundException("User not found");


            var result = await _userManager.DeleteAsync(user);


            if (!result.Succeeded)
                throw new Exception("Failed to delete user");
        }

    }
}
