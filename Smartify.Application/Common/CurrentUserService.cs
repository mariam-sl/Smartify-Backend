using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Smartify.Application.Common
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;


        public bool IsAdmin =>
    _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;

        public bool IsInstructor =>
            _httpContextAccessor.HttpContext?.User.IsInRole("Instructor") ?? false;

        public bool IsStudent =>
            _httpContextAccessor.HttpContext?.User.IsInRole("Student") ?? false;
    }
}
