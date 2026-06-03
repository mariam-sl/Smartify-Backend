using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smartify.Application.Common;
using Smartify.Application.IService;
using Smartify.Domain.Constants;

namespace Smartify.API.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly CurrentUserService _currentUser;

        public EnrollmentController(IEnrollmentService enrollmentService, CurrentUserService currentUser)
        {
            _enrollmentService = enrollmentService;
            _currentUser = currentUser;
        }

        [Authorize(Roles = $"{Roles.Student}")]
        [HttpPost("/api/courses/{courseId}/enroll")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = _currentUser.UserId;

            await _enrollmentService.EnrollAsync(courseId, userId!);

            return Ok(new { message = "Enrolled successfully" });
        }

        [Authorize(Roles = $"{Roles.Student}")]
        [HttpGet("my")]
        public async Task<IActionResult> MyEnrollments()
        {
            var userId = _currentUser.UserId;

            var result =await _enrollmentService.GetUserEnrollmentsAsync(userId!);

            return Ok(result);
        }

        [Authorize(Roles = $"{Roles.Student}")]
        [HttpGet("/api/courses/{courseId}/progress")]
        public async Task<IActionResult> GetProgress(int courseId)
        {
            var userId = _currentUser.UserId;

            var result =await _enrollmentService.GetCourseProgressAsync(courseId, userId!);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = $"{Roles.Student}")]
        [HttpGet("/api/courses/{courseId}/continue")]
        public async Task<IActionResult> Continue(int courseId)
        {
            var userId = _currentUser.UserId;

            var result = await _enrollmentService.GetContinueLessonAsync(courseId, userId!);

            if (result == null)
                return Ok(new { message = "Course completed" });

            return Ok(result);
        }

    }
}
