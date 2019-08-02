using System;
using SB014.API.Models;

namespace SB014.API.BAL
{
    public interface IGameLogic
    {
        GameModel BuildGame(Guid id);
    }
}