using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SB014.API.DAL;
using System;
using SB014.API.BAL;
using SB014.API.Domain;
using Microsoft.AspNetCore.JsonPatch;
using SB014.API.Domain.Enums;
using SB014.API.Models;
using Microsoft.AspNetCore.SignalR;
using SB014.API.Notifications;

namespace SB014.UnitTests.Api
{
    public class Tournament_UpdateTournamentStateShould
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
            var actionResult = tournamentController.Update(Guid.NewGuid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.PrePlay));

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

         [Fact]
         public void ApplyTournamentPreplayLogicRules_WhenStateSetToPreplay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{
                                                                                            Id = new Guid(),
                                                                                            PreplayGameId = null,
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = null,
                                                                                            CluesPerGame = 1});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicMock = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicMock.Object, tournamentBroadcastFake.Object);
            
            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.PrePlay));
            Game newPreplayGame;
            tournamnetLogicMock.Verify(mocks=>mocks.SetPreplay(It.IsAny<Tournament>(),out newPreplayGame), Times.Once);
         }
         [Fact]
         public void ApplyTournamentPreplayLogicRules_WhenStateSetToInplayFromNoplay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{
                                                                                            Id = new Guid(),
                                                                                            PreplayGameId = null,
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = null,
                                                                                            CluesPerGame = 1});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicMock = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicMock.Object, tournamentBroadcastFake.Object);
            
            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.InPlay));
            
            // Assert
            Game newPreplayGame;
            tournamnetLogicMock.Verify(mocks=>mocks.SetPreplay(It.IsAny<Tournament>(),out newPreplayGame), Times.Once);
         }
        [Fact]
         public void ApplyTournamentInplayLogicRules_WhenStateSetToInplay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{
                                                                                            Id = new Guid(),
                                                                                            PreplayGameId = Guid.NewGuid(),
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = Guid.NewGuid(),
                                                                                            CluesPerGame = 1});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicMock = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicMock.Object, tournamentBroadcastFake.Object);
            
            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.InPlay));
            
            // Assert
            Game newPreplayGame;
            tournamnetLogicMock.Verify(mocks=>mocks.SetInplay(It.IsAny<Tournament>(), out newPreplayGame), Times.Once);
         }
         [Fact]
         public void SaveTournament()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var tournament = new Tournament{                                                                                            Id = new Guid(),
                                            PreplayGameId = null,
                                            InplayGameId = null,
                                            PostplayGameId = null,
                                            CluesPerGame = 1};
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(tournament);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);
            TournamentState currentState = tournament.State;

            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.PrePlay));
            tournamentRepositoryMock.Verify(mocks=>mocks.Update(It.IsAny<Tournament>()), Times.Once);
         }
         [Fact]
         public void SaveNewPreplayGame_WhenNewPrePlayGameCreatedFromSettingPrePlay()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();

            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{Id = new Guid(),
                                                                                            PreplayGameId = null,
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = null,
                                                                                            CluesPerGame = 1}); // State no play
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            Game newPreplayGame = new Game();
            tournamnetLogicFake.Setup(x=>x.SetPreplay(It.IsAny<Tournament>(),out newPreplayGame)).Returns(new Tournament{Id = new Guid(),
                                                                                            PreplayGameId = Guid.NewGuid(),
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = null,
                                                                                            CluesPerGame = 1}); 
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.PrePlay));
            tournamentRepositoryMock.Verify(mocks=>mocks.UpdateGame(It.IsAny<Game>()), Times.Once);
         }     
         [Fact]
         public void SaveNewPreplayGame_WhenNewPrePlayGameCreatedFromSettingInPlay()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();

            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{Id = new Guid(),
                                                                                            PreplayGameId = Guid.NewGuid(),
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = Guid.NewGuid(),
                                                                                            CluesPerGame = 1}); // State no play
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            Game newPreplayGame = new Game();
            tournamnetLogicFake.Setup(x=>x.SetInplay(It.IsAny<Tournament>(),out newPreplayGame)).Returns(new Tournament{Id = new Guid(),
                                                                                            PreplayGameId = Guid.NewGuid(),
                                                                                            InplayGameId = Guid.NewGuid(),
                                                                                            PostplayGameId = null,
                                                                                            CluesPerGame = 1}); 
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object, tournamentBroadcastFake.Object);

            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<TournamentStateUpdateModel>().Replace(t=>t.State,TournamentState.InPlay));
            tournamentRepositoryMock.Verify(mocks=>mocks.UpdateGame(It.IsAny<Game>()), Times.Once);
         }          
    }
}