using System;
using System.Collections.Generic;
using SB014.API.Domain;
using SB014.API.Models;
namespace SB014.API.BAL
{
    public interface IGameLogic
    {
        Game BuildGame(Guid id, int cluesPerGame);
        bool EvaluateSubscriberAnswer(string answerAttempt, Clue clue, out int score);
       
        List<SubscriberRankModel> BuildGameRankings(List<SubscriberGameResult> gameResults, int? maxRankingsCount);
    }


}