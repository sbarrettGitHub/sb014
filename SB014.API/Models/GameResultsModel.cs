using System;
using System.Collections.Generic;

namespace SB014.API.Models
{
    public class GameResultsModel
    {               
        public Guid TournamentId { get; set; }   
        public Guid GameId { get; set; }
        public DateTime Created { get; set; }
        public List<SubscriberRankModel> Rankings { get; set; }
        public List<ClueAnswerModel> ClueAnswers { get; set; }
    }
}