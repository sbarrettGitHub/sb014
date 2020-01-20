using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SB014.API.DAL;
using System;
using SB014.API.BAL;
using SB014.API.Domain;
using Microsoft.AspNetCore.SignalR;
using SB014.API.Notifications;

namespace SB014.UnitTests.Api
{
    public class Tournament_UnsubscribeFormTournamentShould
    {
        [Fact]
        public void RemoveSubscriberFromTournament_WhenTournamentAndSubscriberExist()
        {
            // Arrange 
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryMock.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns(new Subscriber {Id = new Guid()});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act
            tournamentController.UnsubscribeFromTournament(new Guid(),new Guid());
        
            // Assert
            tournamentRepositoryMock.Verify(mock => mock.RemoveSubscriber(It.IsAny<Guid>(),It.IsAny<Guid>()), Times.Once());
        }
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
            var  actionResult = tournamentController.UnsubscribeFromTournament(new Guid(),new Guid());

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
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<Subscriber>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var  actionResult = tournamentController.UnsubscribeFromTournament(new Guid(),new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNoContent_WhenTournamentAndSubscriberExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns(new Subscriber {Id = new Guid()});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act 
            var  actionResult = tournamentController.UnsubscribeFromTournament(new Guid(),new Guid());

            // Assert
            Assert.IsType<NoContentResult>(actionResult);
        }
    }
}