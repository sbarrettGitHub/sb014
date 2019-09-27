using System;

namespace SB014.API.Models
{
    public class TournamentModel
    {
        public Guid Id { get; set; }
        public Guid? PreplayGameId { get; set; }
        public Guid? InplayGameId { get; set; }
        public Guid? PostplayGameId { get; set; }
    }
}