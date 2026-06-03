using Smartify.Application.DTO.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDTO>> GetAllQuizzesAsync();
        Task<QuizDTO?> GetQuizByIdAsync(int id);
        Task<QuizDTO?> AddQuizAsync(CreateQuizDTO quizDTO);
        Task UpdateQuizAsync(UpdateQuizDTO quizDTO);
        Task DeleteQuizAsync(int id);
    }
}
