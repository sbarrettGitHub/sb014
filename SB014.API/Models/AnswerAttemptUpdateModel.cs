using System;

namespace SB014.API.Models
{
    public class AnswerAttemptUpdateModel
    {
        public Guid ClueId { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}