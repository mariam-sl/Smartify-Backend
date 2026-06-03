using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartify.Application.Common;
using Smartify.Application.DTO.Lesson;
using Smartify.Application.IService;
using Smartify.Domain.Constants;

namespace Smartify.API.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly CurrentUserService _currentUser;

        public LessonController(ILessonService lessonService, CurrentUserService currentUser)
        {
            _lessonService = lessonService;
            _currentUser = currentUser;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetLesson(int id)
        {
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            var isInstructor = User.IsInRole("Instructor");
            var lesson = await _lessonService.GetLessonDetailsAsync(id, userId!,isAdmin,isInstructor);
            
            return Ok(lesson);
        }

        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        [HttpPost]
        public async Task<IActionResult> CreateLesson(CreateLessonDTO dto)
        {
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            var lesson = await _lessonService.CreateLessonAsync(dto, userId!, isAdmin);
            return Ok(lesson);
        }

        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id,UpdateLessonDTO dto)
        {
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            var lesson = await _lessonService.UpdateLessonAsync(id, dto, userId!, isAdmin);
            if (lesson == null)
                return NotFound();
            return Ok(lesson);
        }

        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            await _lessonService.DeleteLessonAsync(id, userId!, isAdmin);
            return NoContent();
        }

        [Authorize(Roles = $"{Roles.Student}")]
        [HttpPost("{lessonId}/complete")]
        public async Task<IActionResult> CompleteLesson(int lessonId)
        {
            var userId = _currentUser.UserId;
            await _lessonService.MarkLessonCompletedAsync(lessonId, userId!);
            return Ok(new { message = "Lesson Completed" });

        }
    }
}
