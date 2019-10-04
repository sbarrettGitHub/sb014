using System;
using System.Collections.Generic;

namespace SB014.API.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }        
        public DateTime Created { get; set; }

        public List<ClueModel> Clues { get; set; }
    }
}