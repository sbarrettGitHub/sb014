
using System;

namespace SB014.Api.Entities
{
    public class Subscriber
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string Name { get; set; }
    }


}