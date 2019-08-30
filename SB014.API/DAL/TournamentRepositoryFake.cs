using System;
using System.Collections.Generic;
using System.Linq;
using SB014.API.Domain;

namespace SB014.API.DAL
{
    
    public class TournamentRepositoryFake:ITournamentRepository
    {
        public static List<Tournament> Tournaments {get;} = new List<Tournament>();
        public static List<Subscriber> TournamentSubscribers {get;} = new List<Subscriber>();
        public static List<Game> TournamentGames {get;} = new List<Game>();
        public TournamentRepositoryFake()
        {
            TournamentRepositoryFake.Tournaments.Add(
                    new Tournament 
                    {
                        Id = Guid.NewGuid(),
                        CluesPerGame = 20
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

        public bool HasGame(Guid id)
        {
            var t = this.Get(id);

            return t != null && t.PreplayGameId.HasValue;
        }

        public void RemoveSubscriber(Guid tournamentId, Guid id)
        {
            var tournament = TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==tournamentId);
            if(tournament==null)
            {
                throw new Exception("Tournament not found!");
            }
            var subscriber = TournamentRepositoryFake.TournamentSubscribers.FirstOrDefault(ts=>ts.TournamentId == tournamentId && ts.Id == id);
            if(subscriber == null)
            {
                throw new Exception("Tournament subscriber not found!");
            }

            TournamentRepositoryFake.TournamentSubscribers.Remove(subscriber);
        }
        public Game UpdateGame(Game game)
        {
            if(game.GameId == Guid.Empty)
            {
                game.GameId = new Guid();
            }
            return game;
        }
    }
}
