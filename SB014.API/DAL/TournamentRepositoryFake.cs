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
            if(TournamentRepositoryFake.TournamentGames != null && TournamentRepositoryFake.TournamentGames.Count == 0)
            {
             TournamentRepositoryFake.Tournaments.Add(
                    new Tournament 
                    {
                        Id = Guid.NewGuid(),
                        CluesPerGame = 20
                    }
                );               
            }
        }

        public List<Tournament> GetAll()
        {
            return new List<Tournament>(TournamentRepositoryFake.Tournaments);
        }

        public Tournament Get(Guid id)
        {
            return TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==id);

        }
        public Tournament Update(Tournament tournament)
        {
            var tournamentUpdate = TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==t.Id);
            if(tournamentUpdate==null)
            {
                throw new Exception("Tournament not found!");
            }
            tournamentUpdate.CluesPerGame = tournament.CluesPerGame;
            tournamentUpdate.InplayGameId = tournament.InplayGameId;
            tournamentUpdate.PostplayGameId = tournament.PostplayGameId;
            tournamentUpdate.PreplayGameId = tournament.PreplayGameId;

            return tournamentUpdate;
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
            var tournament = TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==game.TournamentId);
            if(tournament==null)
            {
                throw new Exception("Tournament not found!");
            }

            if(game.Id == Guid.Empty)
            {
                game.Id = Guid.NewGuid();
                TournamentRepositoryFake.TournamentGames.Add(game);
            }
            else
            {
                var existingGame = TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId==game.TournamentId && tg.Id == game.Id);
                if(existingGame==null)
                {
                    throw new Exception("Game not found!");
                }
                existingGame.Clues = game.Clues;
            }
            
            return game;
        }
        public Game GetGame(Guid tournamentId, Guid id)
        {
            return TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId == tournamentId && tg.Id == id);
        }
    }
}
