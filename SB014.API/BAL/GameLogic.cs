using System;
using System.Collections.Generic;
using SB014.API.Models;

namespace SB014.API.BAL
{
    public class GameLogic : IGameLogic
    {
        public GameModel BuildGame(Guid guid)
        {
            return new GameModel{
                Anagrams = new List<string>{"one", "two"}
            };
        }
    }

}