using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smartify.Application.Common;
using Smartify.Application.DTO.Course;
using Smartify.Application.IService;
using Smartify.Domain.Constants;

namespace Smartify.API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILessonService _lessonService;
        private readonly CurrentUserService _currentUser;
        public CourseController(ICourseService courseService,ILessonService lessonService, CurrentUserService currentUser)
        {
            _courseService = courseService;
            _lessonService = lessonService;
            _currentUser = currentUser;
           
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublishedCourses()
        {
            var courses = await _courseService.GetPublishedCoursesAsync();

            return Ok(courses);
        }

        [HttpGet("instructor")]
        [Authorize(Roles = $"{Roles.Instructor}")]
        public async Task<IActionResult> GetInstructorCourses()
        {
            var userId = _currentUser.UserId!;

            var courses =await _courseService.GetInstructorCoursesAsync(userId);

            return Ok(courses);
        }

        [HttpGet("admin")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return Ok(courses);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseDetails(int id)
        {
            var course = await _courseService.GetCourseDetailsAsync(id);
            
            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO dto)
        {
            /*string userId = "aa6fd782-8dde-43f4-9d7a-b43502965878"; */
            var userId = _currentUser.UserId;
            var createdCourse = await _courseService.CreateCourseAsync(dto, userId);
                return CreatedAtAction(nameof(GetCourseDetails), new { id = createdCourse.Id }, createdCourse);
            
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDTO dto)
        {
            /*string userId = "aa6fd782-8dde-43f4-9d7a-b43502965878"; */
            var userId = _currentUser.UserId;
            var isAdmin=User.IsInRole("Admin");
            var updatedCourse = await _courseService.UpdateCourseAsync(id, dto, userId,isAdmin);
            return Ok(updatedCourse);
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            /*string userId = "aa6fd782-8dde-43f4-9d7a-b43502965878";*/
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            await _courseService.DeleteCourseAsync(id, userId, isAdmin);
            return NoContent();
            
        }

        [HttpPut("{id}/publish")]
        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        public async Task<IActionResult> PublishCourse(int id)
        {
            /*string userId = "aa6fd782-8dde-43f4-9d7a-b43502965878";*/
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            await _courseService.PublishAsync(id, userId, isAdmin);
                return NoContent();
            
        }

        [HttpPut("{id}/unpublish")]
        [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
        public async Task<IActionResult> UnpublishCourse(int id)
        {
            /*string userId = "aa6fd782-8dde-43f4-9d7a-b43502965878";*/
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            await _courseService.UnpublishAsync(id, userId, isAdmin);
            return NoContent();
           
        }

        [HttpGet("{courseId}/lessons")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseLessons(int courseId)
        {
            var lessons = await _lessonService.GetCourseLessonsAsync(courseId);
            return Ok(lessons);
        }

        [Authorize]
        [HttpGet("{courseId}/lessons/progress")]
        public async Task<IActionResult> GetCourseLessonWithProgress(int courseId)
        {
            var userId = _currentUser.UserId;
            var isAdmin = User.IsInRole("Admin");
            var isInstructor = User.IsInRole("Instructor");

            var lessons= await _lessonService.GetCourseLessonsWithProgressAsync(courseId,userId!, isAdmin,isInstructor);

            return Ok(lessons);

        }
    }
}
