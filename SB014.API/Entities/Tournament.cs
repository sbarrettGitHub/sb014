using System;

namespace SB014.API.Entities
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int GameStatusId { get; set; }
    }


}