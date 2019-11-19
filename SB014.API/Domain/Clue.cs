using System;

namespace SB014.API.Domain
{
    public class Clue
    {
         public Guid Id { get; set; }
        public string GameClue { get; set; }
        public string Answer { get; set; }
    }

}