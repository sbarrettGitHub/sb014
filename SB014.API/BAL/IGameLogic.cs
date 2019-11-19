using System;
using SB014.API.Domain;

namespace SB014.API.BAL
{
    public interface IGameLogic
    {
        Game BuildGame(Guid id, int cluesPerGame);
        bool EvaluateSubscriberAnswer(string answerAttempt, Clue clue, out int score);
    }
}