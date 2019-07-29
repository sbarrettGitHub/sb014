using System;
using System.Collections.Generic;
using SB014.Api.Entities;

namespace SB014.Api.Repository
{
    public interface ITournamentRepository
    {
        List<Tournament> GetAll();
        Tournament Get(Guid id);


        Subscriber AddSubscriber(Subscriber subscriber);
        Subscriber GetSubscriber(Guid tournamentId, Guid id);
    }

}