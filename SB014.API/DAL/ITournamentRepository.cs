using System;
using System.Collections.Generic;
using SB014.API.Domain;

namespace SB014.API.DAL
{
    public interface ITournamentRepository
    {
        List<Tournament> GetAll();
        Tournament Get(Guid id);


        Subscriber AddSubscriber(Subscriber subscriber);
        Subscriber GetSubscriber(Guid tournamentId, Guid id);
        bool HasGame(Guid guid);
        void RemoveSubscriber(Guid tournamentId, Guid id);
        Game UpdateGame(Game game);
    }

}