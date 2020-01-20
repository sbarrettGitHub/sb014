using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SB014.API.Domain.Enums;

namespace SB014.API.Notifications
{
    public class TournamentBroadcast : ITournamentBroadcast
    {
        private readonly IHubContext<TournamentHub> TournamentHub;
        public TournamentBroadcast(IHubContext<TournamentHub> tournamentHub)
        {
            this.TournamentHub = tournamentHub;
        }
        public async Task TournamentStateChangeAsync(Guid id, TournamentState state, Guid? preplayGameId, Guid? inplayGameId, Guid? postplayGameId)
        {
             await this.TournamentHub.Clients.Group(id.ToString()).SendAsync("TournamentUpdate", new 
                                                                                                    {
                                                                                                        Id = id,
                                                                                                        State = state,
                                                                                                        PreplayGameId = preplayGameId,
                                                                                                        InplayGameId = inplayGameId,
                                                                                                        PostplayGameId = postplayGameId,
                                                                                                    });
        }
    }
}