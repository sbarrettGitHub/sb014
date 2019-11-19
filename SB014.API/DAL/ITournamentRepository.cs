using System;
using System.Collections.Generic;
using SB014.API.Domain;

namespace SB014.API.DAL
{
    public interface ITournamentRepository
    {
        List<Tournament> GetAll();
        Tournament Get(Guid id);
        Tournament Update(Tournament t);

        Subscriber AddSubscriber(Subscriber subscriber);
        Subscriber GetSubscriber(Guid tournamentId, Guid id);
        bool HasGame(Guid guid);
        void RemoveSubscriber(Guid tournamentId, Guid id);
        Game UpdateGame(Game game);
        Game GetGame(Guid tournamentId, Guid id);
        void UpdateSubscriberGameResult(Guid tournamentId, Guid subscriberId, Guid gameId, Guid clueId, string answer, int score);
        SubscriberGameResult GetSubscriberGameResult(Guid tournamentId, Guid subscriberId, Guid gameId);
    }

}