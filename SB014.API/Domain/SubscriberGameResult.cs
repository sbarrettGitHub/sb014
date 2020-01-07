
using System;
using System.Collections.Generic;
using System.Linq;

namespace SB014.API.Domain
{
    public class SubscriberGameResult
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid GameId { get; set; }
        public Guid SubscriberId { get; set; }

        public List<AnswerAttempt> AnswerAttempts { get; set; }

        public int GameScore 
        {
            get
            {
                if(AnswerAttempts != null)
                {
                    return AnswerAttempts.Sum(a=>a.Score);
                }
                return 0;
            }
        }
    }


}