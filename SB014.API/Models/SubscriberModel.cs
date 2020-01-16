using System;
using System.ComponentModel.DataAnnotations;

namespace SB014.API.Models
{
    public class SubscriberModel
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string Name { get; set; }
        public string CountryCode {get; set;}
    }
}