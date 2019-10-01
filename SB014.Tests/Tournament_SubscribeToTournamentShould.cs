using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SB014.API.DAL;
using System;
using SB014.API.Models;
using SB014.API.BAL;
using SB014.API.Domain;

namespace SB014.UnitTests.Api
{
    public class Tournament_SubscribeToTournamentShould
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
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusBadRequest_WhenInsufficientDataSupplied()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
            tournamentController.ModelState.AddModelError("fakeError", "fakeError");
            
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), null);
            
            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public void AddSubscriberToRepository()
        {
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryMock.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
    
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            tournamentRepositoryMock.Verify(mock => mock.AddSubscriber(It.IsAny<Subscriber>()), Times.Once());
            
        }
        [Fact]
        public void ReturnSubscriberCreatedAtRoute()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);
     
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            Assert.IsType<CreatedAtRouteResult>(actionResult); 
            
        }
        [Fact]
        public void BuildNewTournamentGame_WhenNoGameExists()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.HasGame(It.IsAny<Guid>())).Returns(false);
            tournamentRepositoryFake.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var mapper = Helper.SetupMapper();
            var gameLogicMock = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicMock.Object, tournamnetLogicFake.Object);

            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="tests"});
            
            // Assert
            gameLogicMock.Verify(mock => mock.BuildGame(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once());
        }
        [Fact]
        public void BuildNewTournamentGameWithStatusPrePlay_WhenNoGameExists()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryMock.Setup(p=>p.HasGame(It.IsAny<Guid>())).Returns(false);
            tournamentRepositoryMock.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var wordReposFake = new Mock<IWordRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogic = new GameLogic(wordReposFake.Object);
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogic, tournamnetLogicFake.Object);

            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="tests"});
            
            // Assert
            tournamentRepositoryMock.Verify(mock => mock.UpdateGame(It.IsAny<Game>()), Times.Once());
        }
        [Fact]
        public void SaveNewTournamentGame_WhenNoGameExists()
        {
            //Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryMock.Setup(p=>p.HasGame(It.IsAny<Guid>())).Returns(false);
            tournamentRepositoryMock.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});
            
            // Assert
            tournamentRepositoryMock.Verify(mock => mock.UpdateGame(It.IsAny<Game>()), Times.Once());
        }
    }
} 