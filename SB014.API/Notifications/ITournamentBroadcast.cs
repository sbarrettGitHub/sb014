using System;
using System.Threading.Tasks;
using SB014.API.Domain.Enums;

namespace SB014.API.Notifications
{
    public interface ITournamentBroadcast
    {
        Task TournamentStateChangeAsync(Guid id, TournamentState state, Guid? preplayGameId, Guid? inplayGameId, Guid? postplayGameId);
    }
}