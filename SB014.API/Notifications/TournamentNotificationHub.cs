using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SB014.API.Notifications
{
    public class TournamentHub: Hub
    {       
        public async Task SubscribeToTournamentNotifications(Guid tournamentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tournamentId.ToString());
        }
        public async Task UnsubscribeFromTournamentNotifications(Guid tournamentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tournamentId.ToString());
        }
    }
}