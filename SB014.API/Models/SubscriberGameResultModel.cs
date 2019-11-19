using System;
using System.Collections.Generic;

namespace SB014.API.Models
{
    public class SubscriberGameResultModel
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid GameId { get; set; }
        public Guid SubscriberId { get; set; }
        public List<AnswerAttemptModel> AnswerAttempts { get; set; }
    }
}