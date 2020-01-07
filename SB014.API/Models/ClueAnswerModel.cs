using System;

namespace SB014.API.Models
{
    public class ClueAnswerModel
    {
        public Guid Id { get; set; }
        public string GameClue { get; set; }
        public string Answer { get; set; }
    }
}