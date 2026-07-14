using Smartify.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IUserService
    {
        Task<UserProfileDTO> GetCurrentUserAsync(string userId);
        Task<InstructorDTO> CreateInstructorAsync(CreateInstructorDTO dto);
        Task<IEnumerable<InstructorDTO>> GetInstructorsAsync();
    }
}
