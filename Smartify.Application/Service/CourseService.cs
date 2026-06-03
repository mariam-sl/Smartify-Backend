using Smartify.Application.DTO.Course;
using Smartify.Application.DTO.Lesson;
using Smartify.Application.IRepository;
using Smartify.Application.IService;
using Smartify.Domain.Entities;
using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseListItemDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(c => new CourseListItemDTO
            {
                Id = c.Id,
                Title = c.Title,
                ShortDescription = c.ShortDescription,
                Difficulty = c.Difficulty,
                Category = c.Category,
                Thumbnail = c.Thumbnail,
                IsPublished = c.IsPublished
            });
        }

        public async Task<CourseDetailsDTO?> GetCourseDetailsAsync(int courseId)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");
            return new CourseDetailsDTO
            {
                Id = course.Id,
                Title = course.Title,
                ShortDescription = course.ShortDescription,
                LongDescription = course.LongDescription,
                Difficulty = course.Difficulty,
                Category = course.Category,
                Thumbnail = course.Thumbnail,
                CreatedAt = course.CreatedAt,
                IsPublished = course.IsPublished,
                Lessons = course.Lessons.Select(l => new LessonListItemDTO
                {
                    Id = l.Id,
                    Title = l.Title,
                    OrderNumber = l.OrderNumber
                    
                })
                .OrderBy(l => l.OrderNumber)
                .ToList()
            };

        }


        public async Task<CourseDTO> CreateCourseAsync(CreateCourseDTO dto, string userId)
        {
            var course = new Course
            {
                Title = dto.Title,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                Difficulty = dto.Difficulty,
                Category = dto.Category,
                Thumbnail = dto.Thumbnail,
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow,
                IsPublished = false
            };

            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            return new CourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                ShortDescription = course.ShortDescription,
                Difficulty = course.Difficulty,
                Category = course.Category,
                IsPublished = course.IsPublished
            };
        }

        public async Task<CourseDTO?> UpdateCourseAsync(int id, UpdateCourseDTO dto, string userId,bool isAdmin)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            //later replace this with proper authorization check
            if (course.CreatedById != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to update this course.");

            if (dto.Title != null)
                course.Title = dto.Title;

            if (dto.ShortDescription != null)
                course.ShortDescription = dto.ShortDescription;

            if (dto.LongDescription != null)
                course.LongDescription = dto.LongDescription;

            if (dto.Category.HasValue)
                course.Category = dto.Category.Value;

            if (dto.Difficulty.HasValue)
                course.Difficulty = dto.Difficulty.Value;

            if (dto.Thumbnail != null)
                course.Thumbnail = dto.Thumbnail;


            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();

            return new CourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                ShortDescription = course.ShortDescription,
                Difficulty = course.Difficulty,
                Category = course.Category,
                IsPublished = course.IsPublished
            };

        }

        public async Task DeleteCourseAsync(int id, string userId,bool isAdmin )
        {
            var course = await _courseRepository.GetDetailedByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");
            //later replace with proper authorization 
            if (course.CreatedById != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to delete this course.");
            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task PublishAsync(int id, string userId,bool isAdmin)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");
            //later replace with proper authorization
            if (course.CreatedById != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to publish this course.");
            if (course.IsPublished)
                throw new InvalidOperationException("Course is already published.");
            //optional future business rule
            //if(!course.Lessons.Any())
            //    throw new InvalidOperationException("Course must have at least one lesson before publishing.");
            course.IsPublished = true;
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task UnpublishAsync(int id, string userId, bool isAdmin)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");
            //later replace with proper authorization
            if (course.CreatedById != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to unpublish this course.");
            if (!course.IsPublished)
                throw new InvalidOperationException("Course is already unpublished.");
            course.IsPublished = false;
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();


        }
    }
}
