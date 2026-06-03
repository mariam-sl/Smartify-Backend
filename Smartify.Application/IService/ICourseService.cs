using Smartify.Application.DTO.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListItemDTO>> GetAllCoursesAsync();

        Task<CourseDetailsDTO?> GetCourseDetailsAsync(int courseId);

       /* Task<IEnumerable<CourseListItemDTO>> GetCoursesByAuthorAsync(string authorId);

        Task<IEnumerable<CourseListItemDTO>> SearchCoursesAsync(string searchTerm);

        Task<IEnumerable<CourseListItemDTO>> GetFeaturedCoursesAsync();*/

        Task<CourseDTO> CreateCourseAsync(CreateCourseDTO dto,string userId);

       Task<CourseDTO?> UpdateCourseAsync(int id,UpdateCourseDTO dto,string userId,bool isAdmin);

        Task DeleteCourseAsync(int id,string userId,bool isAdmin);

       Task PublishAsync(int id,string userId,bool isAdmin);

         Task UnpublishAsync(int id,string userId,bool isAdmin);


    }
}
