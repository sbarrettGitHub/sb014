using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SB014.API.Hubs
{
    public class TournamentHub: Hub
    {       
        public async Task SubscribeToTournamentNotifications(Guid tournamentId)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, tournamentId.ToString());
        }
    }
}