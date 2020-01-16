
using System;

namespace SB014.API.Domain
{
    public class Subscriber
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string Name { get; set; }
        public string CountryCode {get; set;}
    }


}