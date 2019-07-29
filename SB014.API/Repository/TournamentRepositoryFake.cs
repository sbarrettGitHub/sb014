using System;
using System.Collections.Generic;
using System.Linq;
using SB014.Api.Entities;

namespace SB014.Api.Repository
{
    
    public class TournamentRepositoryFake:ITournamentRepository
    {
        public static List<Tournament> Tournaments {get;} = new List<Tournament>();
        public static List<Subscriber> TournamentSubscribers {get;} = new List<Subscriber>();

        public TournamentRepositoryFake()
        {
            TournamentRepositoryFake.Tournaments.Add(
                    new Tournament 
                    {
                        Id = Guid.NewGuid()
                    }
                );
        }

        public List<Tournament> GetAll()
        {
            return new List<Tournament>(TournamentRepositoryFake.Tournaments);
        }

        public Tournament Get(Guid id)
        {
            return TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==id);

        }

        public Subscriber AddSubscriber(Subscriber subscriber)
        {
            subscriber.Id = Guid.NewGuid();
            TournamentRepositoryFake.TournamentSubscribers.Add(subscriber);
            return subscriber;
        }

        public Subscriber GetSubscriber(Guid tournamentId, Guid id)
        {
            return TournamentRepositoryFake.TournamentSubscribers.FirstOrDefault(ts=>ts.TournamentId == tournamentId && ts.Id == id);
        }
    }
}
