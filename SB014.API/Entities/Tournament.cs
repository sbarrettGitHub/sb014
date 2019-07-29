using System;

namespace SB014.Api.Entities
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int GameStatusId { get; set; }
    }


}