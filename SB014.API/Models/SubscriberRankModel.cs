using System;

namespace SB014.API.Models
{
    public class SubscriberRankModel
    {
        public Guid SubscriberId { get; set; }
        public string Name { get; set; }
        public string CountryCode {get; set;}
        public int Rank { get; set; }
        public int GameScore{ get; set; }
    }
}