using System;

namespace SB014.API.Domain
{
    public class AnswerAttempt
    {
        public Guid Id { get; set; }
        public Guid ClueId { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }

}