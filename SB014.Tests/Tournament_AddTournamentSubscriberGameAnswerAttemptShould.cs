using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SB014.API.BAL;
using SB014.API.Controllers;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Models;
using Xunit;

namespace SB014.UnitTests.Api
{
    public class Tournament_AddTournamentSubscriberGameAnswerAttemptShould
    {
        public delegate void EvaluateSubscriberAnswerFake(string ansAttempt, Clue clue, out int s);

        [Fact]
        public void ReturnStatusNotFound_WhenTournamentDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns<Tournament>(null);
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());            
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), new Guid(), answerAttempt);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<Game>(null);
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), new Guid(), answerAttempt);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game{Id=gameId});
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<Subscriber>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), new Guid(), answerAttempt);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }    
        [Fact]
        public void ReturnStatusNotFound_WhenGameClueDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});            
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game{Id=gameId, Clues = new List<Clue>()});
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), new Guid(), answerAttempt);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }    
        [Fact]
        public void EvaluateSubscribeAnswerAttempt()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId = gameId});            
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Game{
                    Id = gameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId
                        }
                    }
            });
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicMock = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicMock.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), clueId, answerAttempt);

            // Assert
            int score;
            gameLogicMock.Verify(mocks=>mocks.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score), Times.Once);
        } 
        [Fact]
        public void ReturnStatusOK_WhenAnswerIsCorrect()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});            
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Game{
                    Id=gameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId
                        }
                    }
            });
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            int score;
            gameLogicFake.Setup(x=>x.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score)).Returns(true);
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), clueId, answerAttempt);

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }
        [Fact]
        public void ReturnBadRequest_WhenAnswerIsIncorrect()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});            
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Game{
                    Id = gameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId
                        }
                    }
            });
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            int score;
            gameLogicFake.Setup(x=>x.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score)).Returns(false);
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};
            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), clueId, answerAttempt);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public void RecordSubsciberGameAnswer()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});            
            tournamentRepositoryMock.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Game{
                    Id = gameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId
                        }
                    }
            });
            tournamentRepositoryMock.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            int score=0;
            gameLogicFake.Setup(x=>x.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score)).Returns(true);
            var tournamentLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamentLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), clueId, answerAttempt);

            // Assert
            tournamentRepositoryMock.Verify(x=>x.UpdateSubscriberGameResult(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), clueId, answerAttempt.Answer, score), Times.Once);


        }
                
        [Fact]
        public void RecordSubsciberGameScore()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid gameId = Guid.NewGuid();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId=gameId});            
            tournamentRepositoryMock.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Game{
                    Id = gameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId
                        }
                    }
            });
            tournamentRepositoryMock.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            int score=0;
            int fakeScore = 3;
            
            gameLogicFake.Setup(x=>x.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score))
            .Callback(new EvaluateSubscriberAnswerFake((string ansAttempt, Clue clue, out int s) => {s = fakeScore; }))
            .Returns(true);

            var tournamentLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamentLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};

            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), new Guid(), clueId, answerAttempt);

            // Assert the respoitory was called with the fake score to be persisted 
            tournamentRepositoryMock.Verify(x=>x.UpdateSubscriberGameResult(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), clueId, answerAttempt.Answer, fakeScore), Times.Once);

        }        
       [Fact]
       public void ReturnBadRequest_WhenGameNotInPlay()
       {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid clueId = Guid.NewGuid();
            Guid GameId = Guid.NewGuid();
            // Ensure game of test is PostPlay not InPlay
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament
            {
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = Guid.NewGuid(),
                PostplayGameId = GameId
            });            
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), GameId)).Returns(
                new Game{
                    Id = GameId,
                    Clues = new List<Clue>
                    {
                        new Clue
                        {
                            Id = clueId 
                        }
                    }
            });
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var gameLogicFake = new Mock<IGameLogic>();
            int score;
            gameLogicFake.Setup(x=>x.EvaluateSubscriberAnswer(It.IsAny<string>(), It.IsAny<Clue>(), out score)).Returns(true);
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            AnswerAttemptUpdateModel answerAttempt = new AnswerAttemptUpdateModel{ Answer = "myanswer"};
            // Act 
            var actionResult = tournamentController.AddTournamentSubscriberGameAnswerAttempt(new Guid(), new Guid(), GameId, clueId, answerAttempt);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
       }
    }
}