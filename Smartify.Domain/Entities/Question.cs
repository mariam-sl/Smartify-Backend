using Smartify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class Question 
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public int OrderNumber { get; set; }

       public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<QuizAttemptAnswer> AttemptAnswers { get; set; } = new List<QuizAttemptAnswer>();
    }
}
