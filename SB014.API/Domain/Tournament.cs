using System;

namespace SB014.API.Domain
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public Guid? PreplayGameId { get; set; }
        public Guid? InplayGameId { get; set; }
        public Guid? PostplayGameId { get; set; }
        public int? GameStatusId { get; set; }

        public int CluesPerGame { get; set; }
    }


}