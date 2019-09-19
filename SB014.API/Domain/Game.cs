using System;
using System.Collections.Generic;

namespace SB014.API.Domain
{
    public class Game
    {
        public Game()
        {
            GameStatusId = (int)Enums.GameStatus.PrePlay;
            Clues = new List<Clue>();
        }
        public Guid GameId { get; set; }
        public Guid TournamentId { get; set; }

        public int GameStatusId {get; set;}

        public List<Clue> Clues { get; set; }
    }
}