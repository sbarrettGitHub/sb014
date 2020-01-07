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
    public class Tournament_GetSubscriberGameResultsShould
    {
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

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());

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

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
             tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<Subscriber>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        } 
        [Fact]
        public void ReturnStatusNotFound_WhenSubscriberGameResultsDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
             tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriberGameResult(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<SubscriberGameResult>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }   
        [Fact]
        public void ReturnStatusOk_WhenSubscriberGameResultsExists()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
             tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriberGameResult(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new SubscriberGameResult());
            tournamentRepositoryFake.Setup(p=>p.GetAllSubscriberGameResults(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new List<SubscriberGameResult>());
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(p=>p.BuildGameRankings(It.IsAny<List<SubscriberGameResult>>(),It.IsAny<int?>())).Returns(new List<SubscriberRankModel>());
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }    
        [Fact]
        public void ReturnSubscriberGameResults_WhenSubscriberGameResultsExists()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
             tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Game());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriberGameResult(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new SubscriberGameResult());
            tournamentRepositoryFake.Setup(p=>p.GetAllSubscriberGameResults(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new List<SubscriberGameResult>());
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(p=>p.BuildGameRankings(It.IsAny<List<SubscriberGameResult>>(),It.IsAny<int?>())).Returns(new List<SubscriberRankModel>());
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentSubscriberGameResults(new Guid(), new Guid(), new Guid());
            var contentResult = actionResult as OkObjectResult; 

            // Assert
            Assert.IsType<SubscriberGameResultModel>(contentResult.Value);
            
        }                      
    }
}