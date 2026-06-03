using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Domain.Entities
{
    public class QuizAttempt 
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string UserId { get; set; } // Identity reference
        
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }
        public int DurationSeconds { get; set; }

        public ICollection<QuizAttemptAnswer> Answers { get; set; } = new List<QuizAttemptAnswer>();
    }
}
