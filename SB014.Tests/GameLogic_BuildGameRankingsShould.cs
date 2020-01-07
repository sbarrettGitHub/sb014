using Moq;
using SB014.API.BAL;
using SB014.API.Domain;
using Xunit;
using System.Linq;
using SB014.API.DAL;
using System.Collections.Generic;
using SB014.API.Helpers;
using System;
using SB014.API.Models;

namespace SB014.UnitTests.Api
{
    public class GameLogic_OrderSubscriberGameResultsShould
    {
        [Fact]
        public void OrderResultsByGameScoreHighestFirst()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            var tournamentRepoFake = new Mock<TournamentRepositoryFake>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object, tournamentRepoFake.Object);
            Guid winnerSubscriberId = Guid.NewGuid();
            Guid losingSubscriberId  = Guid.NewGuid();

            List<SubscriberGameResult> gameResults = new List<SubscriberGameResult>
            {
                new SubscriberGameResult{
                        SubscriberId = Guid.NewGuid(),
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 5
                            }
                        }
                },
                new SubscriberGameResult{
                        SubscriberId = losingSubscriberId,
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 0
                            }
                        }
                },
                new SubscriberGameResult{
                        SubscriberId = winnerSubscriberId,
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 100
                            }
                        }
                },
            };
            // Act
            List<SubscriberRankModel> orderedResults = gameLogic.BuildGameRankings(gameResults, 100);
        
            // Assert, that the winning subscriber is at the top of the resulting ordered list
            Assert.True(orderedResults.First().SubscriberId == winnerSubscriberId);
        }
         [Fact]
        public void GiveSameRankToSubscribersWithSameScore()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            var tournamentRepoFake = new Mock<TournamentRepositoryFake>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object, tournamentRepoFake.Object);
            Guid winnerSubscriberId = Guid.NewGuid();
            Guid losingSubscriberId  = Guid.NewGuid();
            Guid winnerSubscriber2Id  = Guid.NewGuid();

            List<SubscriberGameResult> gameResults = new List<SubscriberGameResult>
            {
                new SubscriberGameResult{
                        SubscriberId = winnerSubscriber2Id,
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 100
                            }
                        }
                },
                new SubscriberGameResult{
                        SubscriberId = losingSubscriberId,
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 0
                            }
                        }
                },
                new SubscriberGameResult{
                        SubscriberId = winnerSubscriberId,
                        AnswerAttempts = new List<AnswerAttempt>
                        {
                            new AnswerAttempt
                            {
                                Score = 100
                            }
                        }
                },
            };
            // Act
            List<SubscriberRankModel> orderedResults = gameLogic.BuildGameRankings(gameResults, 100);
        
            // Assert, that the 2 winning subscribers get the same rank
            Assert.True(orderedResults.FirstOrDefault(r=>r.SubscriberId == winnerSubscriberId).Rank == orderedResults.FirstOrDefault(r=>r.SubscriberId == winnerSubscriber2Id).Rank);
        }
    }
}