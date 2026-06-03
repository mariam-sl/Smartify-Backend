using Smartify.Application.DTO.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDTO>> GetAllQuestionsAsync();
        Task<QuestionDTO?> GetQuestionByIdAsync(int id);
        Task<QuestionDTO?> AddQuestionAsync(CreateQuestionDTO questionDTO);
        Task UpdateQuestionAsync(UpdateQuestionDTO questionDTO);
        Task DeleteQuestionAsync(int id);
    }
}
