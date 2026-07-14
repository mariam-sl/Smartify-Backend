using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartify.Application.Common;
using Smartify.Application.DTO.User;
using Smartify.Application.IService;
using Smartify.Domain.Constants;

namespace Smartify.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
            private readonly IUserService _userService;
            public readonly CurrentUserService _currentUser;
            public UserController(IUserService userService,CurrentUserService currentUser)
            {
                _userService = userService;
                _currentUser = currentUser;
            }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = _currentUser.UserId;

            var user = await _userService.GetCurrentUserAsync(userId!);

            return Ok(user);
        }





        [HttpPost("instructors")]
            [Authorize(Roles =Roles.Admin)]
            public async Task<IActionResult> CreateInstructor(CreateInstructorDTO dto)
            {
                var instructor = await _userService.CreateInstructorAsync(dto);
                return Ok(instructor);
            }

            [HttpGet("instructors")]
            [Authorize(Roles = Roles.Admin)]
            public async Task<IActionResult> GetInstructors()
            {
                var instructors = await _userService.GetInstructorsAsync();
                return Ok(instructors);
            }

    }
}
