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
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILessonCompletionRepository _lessonCompletionRepository;

        public LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository, ILessonCompletionRepository lessonCompletionRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _lessonCompletionRepository = lessonCompletionRepository;
        }

        public async Task<IEnumerable<LessonListItemDTO>> GetCourseLessonsAsync(int courseId)
        {
            var course =await _courseRepository.GetDetailedByIdAsync(courseId);

            if (course == null)
                throw new KeyNotFoundException("Course not found");
            var lessons= await _lessonRepository.GetByCourseIdAsync(courseId);
            return lessons.Select(l=>new LessonListItemDTO
                {
                Id = l.Id,
                Title = l.Title,
                OrderNumber = l.OrderNumber
            });
        }

        public async Task<LessonDetailsDTO?> GetLessonDetailsAsync(int lessonId,string userId,bool isAdmin,bool isInstructor)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            if(isAdmin || isInstructor)
            {
                return new LessonDetailsDTO
                {
                    Id=lesson.Id,
                    CourseId = lesson.CourseId,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    VideoUrl = lesson.VideoUrl,
                    OrderNumber = lesson.OrderNumber,
                    CreatedAt = lesson.CreatedAt
                };
            }

            //check enrollment for student
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, lesson.CourseId);
            if(enrollment==null)
                throw new UnauthorizedAccessException("You are not enrolled in this course");

            //first lesson always allowed
            if(lesson.OrderNumber==1)
                return new LessonDetailsDTO
                {
                    Id = lesson.Id,
                    CourseId = lesson.CourseId,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    VideoUrl = lesson.VideoUrl,
                    OrderNumber = lesson.OrderNumber,
                    CreatedAt = lesson.CreatedAt
                };

            //get previous lesson
            var courseLessons = await _lessonRepository.GetByCourseIdAsync(lesson.CourseId);
            var previousLesson= courseLessons
                .Where(l => l.OrderNumber < lesson.OrderNumber)
                .OrderByDescending(l => l.OrderNumber)
                .FirstOrDefault();

            if(previousLesson==null)
                return new LessonDetailsDTO
                {
                    Id = lesson.Id,
                    CourseId = lesson.CourseId,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    VideoUrl = lesson.VideoUrl,
                    OrderNumber = lesson.OrderNumber,
                    CreatedAt = lesson.CreatedAt
                };


            //check if previous lesson is completed
            var previousCompleted = await _lessonCompletionRepository.GetAsync(userId, previousLesson.Id);

            if (previousCompleted == null)
                throw new UnauthorizedAccessException("You must complete previous lesson first.");

            return new LessonDetailsDTO
            {
                Id = lesson.Id,
                CourseId = lesson.CourseId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                OrderNumber = lesson.OrderNumber,
                CreatedAt = lesson.CreatedAt
            };
        }

        public async Task<LessonDTO> CreateLessonAsync(CreateLessonDTO dto,string userId,bool isAdmin)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(dto.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found");
            if (!isAdmin && course.CreatedById != userId)
                throw new UnauthorizedAccessException();
            var lesson = new Lesson
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                Content = dto.Content,
                VideoUrl = dto.VideoUrl,
                OrderNumber = dto.OrderNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _lessonRepository.AddAsync(lesson);
            await _lessonRepository.SaveChangesAsync();

            return new LessonDTO
            {
                Id = lesson.Id,
                CourseId = lesson.CourseId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                OrderNumber = lesson.OrderNumber
            };
        }
       
        public async Task<LessonDTO?> UpdateLessonAsync(int lessonId,UpdateLessonDTO dto,string userId,bool isAdmin)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");
            if (!isAdmin && lesson.Course.CreatedById != userId)
                throw new UnauthorizedAccessException();
            lesson.Title = dto.Title;
            lesson.Content = dto.Content;
            lesson.VideoUrl = dto.VideoUrl;
            lesson.OrderNumber = dto.OrderNumber;

            _lessonRepository.Update(lesson);
            await _lessonRepository.SaveChangesAsync();

            return new LessonDTO
            {
                Id = lesson.Id,
                CourseId = lesson.CourseId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                OrderNumber = lesson.OrderNumber
            };
        }
        

        public async Task DeleteLessonAsync(int lessonId,string userId,bool isAdmin)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");
            if (!isAdmin && lesson.Course.CreatedById != userId)
                throw new UnauthorizedAccessException();
            _lessonRepository.Delete(lesson);
            await _lessonRepository.SaveChangesAsync();
        }



        public async Task MarkLessonCompletedAsync(int lessonId,string userId)
        {
            //get lesson
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            //check enrollment
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, lesson.CourseId);
            if (enrollment == null)
                throw new UnauthorizedAccessException("User not enrolled in course");

            //check if already completed
            var existing = await _lessonCompletionRepository.GetAsync(userId, lessonId);
            if (existing != null)
                throw new InvalidOperationException("Lesson already completed.");

            //create completion
            var completion = new LessonCompletion
            {
                LessonId = lessonId,
                UserId = userId,
                CompletedAt = DateTime.UtcNow

            };

            await _lessonCompletionRepository.AddAsync(completion);
            await _lessonCompletionRepository.SaveChangesAsync();

            //update progress
            await UpdateEnrollmentProgress(userId, lesson.CourseId);
        
        }


        //Progress calculation method
        private async Task UpdateEnrollmentProgress(string userId,int courseId)
        {
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);
            if (enrollment == null)
                return;
            var totalLessons = await _lessonRepository.GetLessonsCountAsync(courseId);
            var completedLessons = await _lessonCompletionRepository.GetCompletedCount(userId, courseId);

            if (totalLessons == 0)
                enrollment.Progress = 0;
            else
                enrollment.Progress = (int)((completedLessons * 100.0) / totalLessons);
            if(enrollment.Progress==100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = DateTime.UtcNow;
            }

            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync();
        }


        public async Task<IEnumerable<LessonListItemDTO>> GetCourseLessonsWithProgressAsync(int courseId,string userId, bool isAdmin,bool isInstructor)
        {
            var course = await _courseRepository.GetDetailedByIdAsync(courseId);
            if(course==null)
                throw new KeyNotFoundException("Course not found");

            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);

            //admin or instructor => everything unlocked
            if(isAdmin||isInstructor)
            {
                return lessons.Select(l => new LessonListItemDTO
                {
                    Id = l.Id,
                    Title = l.Title,
                    OrderNumber = l.OrderNumber,
                    IsLocked = false
                });
            }


            //student must be enrolled
            var enrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);
            if (enrollment == null)
                throw new UnauthorizedAccessException("You are not enrolled in this course");

            //get completed lesson IDs 
            var completedLessonIds = await _lessonCompletionRepository.GetCompletedLessonIds(userId, courseId);

            var result = new List<LessonListItemDTO>();

            foreach (var lesson in lessons)
            {
                //first lesson always unlocked
                if(lesson.OrderNumber==1)
                {
                    result.Add(new LessonListItemDTO
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        OrderNumber = lesson.OrderNumber,
                        IsLocked = false
                    });
                    continue;
                }

                //find previous lesson
                var previousLesson = lessons
                    .Where(l => l.OrderNumber < lesson.OrderNumber)
                    .OrderByDescending(l => l.OrderNumber)
                    .First();

                var isUnlocked = completedLessonIds.Contains(previousLesson.Id);
                result.Add(new LessonListItemDTO
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    OrderNumber = lesson.OrderNumber,
                    IsLocked = !isUnlocked
                });
            }

            return result;

        }


        }

 }
