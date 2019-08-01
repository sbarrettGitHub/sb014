using System;
using System.Collections.Generic;
using SB014.API.Entities;

namespace SB014.API.Repository
{
    public interface ITournamentRepository
    {
        List<Tournament> GetAll();
        Tournament Get(Guid id);


        Subscriber AddSubscriber(Subscriber subscriber);
        Subscriber GetSubscriber(Guid tournamentId, Guid id);
    }

}