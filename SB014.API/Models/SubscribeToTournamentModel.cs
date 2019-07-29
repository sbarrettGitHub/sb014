using System;
using System.ComponentModel.DataAnnotations;

namespace SB014.Api.Models
{
    public class SubscribeToTournamentModel
    {
        
        [Required]
        public string Name { get; set; }
    }
}