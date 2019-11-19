using System;
using System.Collections.Generic;
using System.Linq;
using SB014.API.Domain;
using SB014.API.Domain.Enums;

namespace SB014.API.DAL
{
    
    public class TournamentRepositoryFake:ITournamentRepository
    {
        public static List<Tournament> Tournaments {get;} = new List<Tournament>();
        public static List<Subscriber> TournamentSubscribers {get;} = new List<Subscriber>();
        public static List<Game> TournamentGames {get;} = new List<Game>();
        public static List<SubscriberGameResult> SubscriberGameResults {get;} = new List<SubscriberGameResult>();
        public TournamentRepositoryFake()
        {
            if(TournamentRepositoryFake.TournamentGames != null && TournamentRepositoryFake.TournamentGames.Count == 0)
            {
             TournamentRepositoryFake.Tournaments.Add(
                    new Tournament 
                    {
                        Id = Guid.NewGuid(),
                        CluesPerGame = 20,
                        BellStateLookupMatrix = new List<BellStateLookup>
                        {
                            new BellStateLookup
                            {
                                BellCounter = 1,
                                State = TournamentState.PrePlay
                            },
                            new BellStateLookup
                            {
                                BellCounter = 2,
                                State = TournamentState.InPlay
                            },
                            new BellStateLookup
                            {
                                BellCounter = 3,
                                State = TournamentState.InPlay
                            }                            ,
                            new BellStateLookup
                            {
                                BellCounter = 4,
                                State = TournamentState.PostPlay
                            }
                        }
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
            }
           
            var existingGame = TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId==game.TournamentId && tg.Id == game.Id);
            if(existingGame==null)
            {
                TournamentRepositoryFake.TournamentGames.Add(game);
            }
            else
            {
                existingGame.Clues = game.Clues;
            }  
            
            return game;
        }
        public Game GetGame(Guid tournamentId, Guid id)
        {
            return TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId == tournamentId && tg.Id == id);
        }
        /// <summary>
        /// Add answer to subscriber game result
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="subscriberId"></param>
        /// <param name="gameId"></param>
        /// <param name="clueId"></param>
        /// <param name="answer"></param>
        /// <param name="score"></param>
        public void UpdateSubscriberGameResult(Guid tournamentId, Guid subscriberId, Guid gameId, Guid clueId, string answer, int score)
        {
            var tournament = TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==tournamentId);
            if(tournament==null)
            {
                throw new Exception("Tournament not found!");
            }

            var game = TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId==tournamentId && tg.Id == gameId);
            if(game==null)
            {
                throw new Exception("Game not found!");
            }
            if(game.Clues.FirstOrDefault(c=>c.Id == clueId) == null)
            {
                throw new Exception("Clue not found!");
            }

            var tournamentSubscriber = TournamentRepositoryFake.TournamentSubscribers.FirstOrDefault(ts=>ts.TournamentId == tournamentId && ts.Id == subscriberId);
            if(tournamentSubscriber == null)
            {
                throw new Exception("Tournament subscriber not found!");

            }

            var subscriberGameResults = TournamentRepositoryFake.SubscriberGameResults.FirstOrDefault(sgr=>sgr.TournamentId==tournamentId && sgr.GameId == gameId && sgr.SubscriberId == subscriberId);
            if(subscriberGameResults==null)
            {
                subscriberGameResults = new SubscriberGameResult
                {
                    Id = Guid.NewGuid(),
                    TournamentId = tournamentId,
                    GameId = gameId,
                    SubscriberId = subscriberId,
                    AnswerAttempts = new List<AnswerAttempt>()
                };
                // Add to repository
                TournamentRepositoryFake.SubscriberGameResults.Add(subscriberGameResults);
            }
            var answerAttempt = subscriberGameResults.AnswerAttempts.FirstOrDefault(aa=>aa.ClueId == clueId);
            if(answerAttempt == null)
            {
                answerAttempt = new AnswerAttempt
                {
                    Id = Guid.NewGuid(),
                    ClueId = clueId
                };
                subscriberGameResults.AnswerAttempts.Add(answerAttempt);
            }
            answerAttempt.Answer = answer;
            answerAttempt.Score = score;
        }

        /// <summary>
        /// Get the subcriber results of a game 
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="subscriberId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public SubscriberGameResult GetSubscriberGameResult(Guid tournamentId, Guid subscriberId, Guid gameId)
        {
            var tournament = TournamentRepositoryFake.Tournaments.FirstOrDefault(t=>t.Id==tournamentId);
            if(tournament==null)
            {
                throw new Exception("Tournament not found!");
            }

            var game = TournamentRepositoryFake.TournamentGames.FirstOrDefault(tg=>tg.TournamentId==tournamentId && tg.Id == gameId);
            if(game==null)
            {
                throw new Exception("Game not found!");
            }

            var tournamentSubscriber = TournamentRepositoryFake.TournamentSubscribers.FirstOrDefault(ts=>ts.TournamentId == tournamentId && ts.Id == subscriberId);
            if(tournamentSubscriber == null)
            {
                throw new Exception("Tournament subscriber not found!");
            }
            return TournamentRepositoryFake.SubscriberGameResults.FirstOrDefault(sgr=>sgr.TournamentId==tournamentId && sgr.GameId == gameId && sgr.SubscriberId == subscriberId);
        }
    }
}
