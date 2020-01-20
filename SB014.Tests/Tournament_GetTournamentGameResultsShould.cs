using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SB014.API.BAL;
using SB014.API.Controllers;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Notifications;
using SB014.API.Models;
using Xunit;
namespace SB014.UnitTests.Api
{
    public class Tournament_GetTournamentGameResultsShould
    {
        [Fact]
        public void ReturnStatusNotFound_WhenTournamentDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns<Tournament>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentGameResults(new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenTournamentGameDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns<Game>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentGameResults(new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusBadRequest_WhenTournamentGameIsInplay()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid inPlayGameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament
            {
                InplayGameId = inPlayGameId
            });
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns(new Game{
                Id = inPlayGameId
            });
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentGameResults(new Guid(), new Guid());

            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public void ReturnGameResults_WhenTournamentGameExtistsAndIsNotInplay()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            Guid inPlayGameId = Guid.NewGuid();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{InplayGameId = Guid.NewGuid()});
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns(new Game());
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournamentGameResults(new Guid(), new Guid());
            var contentResult = actionResult as OkObjectResult; 

            // Assert
            Assert.IsType<GameResultsModel>(contentResult.Value);
        }
    }
}