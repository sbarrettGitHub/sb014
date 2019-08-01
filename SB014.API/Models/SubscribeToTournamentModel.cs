using System;
using System.ComponentModel.DataAnnotations;

namespace SB014.API.Models
{
    public class SubscribeToTournamentModel
    {
        
        [Required]
        public string Name { get; set; }
    }
}