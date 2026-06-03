using Smartify.Application.DTO.Enrollment;
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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonCompletionRepository _lessonCompletionRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            ILessonCompletionRepository lessonCompletionRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _lessonCompletionRepository = lessonCompletionRepository;
        }

        public async Task EnrollAsync(int courseId,string userId)
        {
            //check course exists
            var course = await _courseRepository.GetDetailedByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found");

            //check if the user is already enrolled in the course
            var existing = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);
            if (existing != null)
                throw new InvalidOperationException("Already enrolled in the course");

            //create the enrollment
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                Progress = 0,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow
            };

            await _enrollmentRepository.AddAsync(enrollment);
            await _enrollmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetUserEnrollmentsAsync(String userId)
        {
            var enrollments = await _enrollmentRepository.GetByUserAsync(userId);
            return enrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseTitle = e.Course.Title,
                Progress = e.Progress,
                Status = e.Status,
                EnrolledAt = e.EnrolledAt,
                CompletedAt = e.CompletedAt
            });
        }


        public async Task<CourseProgressDTO?> GetCourseProgressAsync(int courseId,string userId)
        {
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);
            if (enrollment == null)
                return null;
            var totalLessons= await _lessonRepository.GetLessonsCountAsync(courseId);
            var completedLessons = await _lessonCompletionRepository.GetCompletedCount(userId, courseId);
            return new CourseProgressDTO
            {
                CourseId = courseId,
                Status = enrollment.Status,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                Progress = (int)((completedLessons * 100.0) / totalLessons)
            };
        }

        public async Task<LessonListItemDTO?> GetContinueLessonAsync(int courseId,string userId)
        {
            //check enrollment
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);
            if (enrollment == null)
                throw new KeyNotFoundException("User is not enrolled in this course. ");

            //get all lessons ordered
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);

            //get completed lessons
            var completedLessons = await _lessonCompletionRepository.GetCompletedLessonIds(userId, courseId);

            //find first incomplete lesson
            var nextLesson = lessons.FirstOrDefault(l => !completedLessons.Contains(l.Id));

            //if everything is completed
            if (nextLesson == null)
                return null;

            //mapping to DTO
            return new LessonListItemDTO
            {
                Id = nextLesson.Id,
                Title = nextLesson.Title,
                OrderNumber = nextLesson.OrderNumber
            };
        }

    }
}
