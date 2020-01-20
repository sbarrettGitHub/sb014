using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SB014.API.BAL;
using SB014.API.Controllers;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Domain.Enums;
using SB014.API.Notifications;
using SB014.API.Models;
using Xunit;

namespace SB014.UnitTests.Api
{
    public class Tournament_AddTournamentStateBellShould
    {
        [Fact]
        public async void ReturnStatusNotFound_WhenTournamentDoesNotExist()
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
            var actionResult = await tournamentController.AddTournamentStateBell(new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
         public void ApplyTournamentStateBellLogicRules()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{
                                                                                            Id = new Guid(),
                                                                                            PreplayGameId = Guid.NewGuid(),
                                                                                            InplayGameId = null,
                                                                                            PostplayGameId = Guid.NewGuid(),
                                                                                            CluesPerGame = 1,
                                                                                            BellCounter = 0});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamentLogicMock = new Mock<ITournamentLogic>();
            tournamentLogicMock.Setup(t=>t.AddBell(It.IsAny<Tournament>())).Returns(new TournamentStateUpdateModel{State = TournamentState.NoPlay});
            var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamentLogicMock.Object, tournamentBroadcastFake.Object);
            
            // Act
            var actionResult = tournamentController.AddTournamentStateBell(new Guid());
            
            // Assert
            tournamentLogicMock.Verify(mocks=>mocks.AddBell(It.IsAny<Tournament>()), Times.Once);
         }
        
        [Fact]
        public void SaveTournament_WhenStateChangesDueToBellCounter()
        {
        // Arrange
        var tournamentRepositoryMock = new Mock<ITournamentRepository>();
        var mapper = Helper.SetupMapper();
        var tournament = 
            new Tournament{ 
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1,
                BellCounter = 0,
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
                    }
                }
            };
        tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(tournament);
        var gameLogicFake = new Mock<IGameLogic>();        
        var tournamentLogicFake = new Mock<ITournamentLogic>();
        tournamentLogicFake.Setup(t=>t.AddBell(It.IsAny<Tournament>())).Returns(new TournamentStateUpdateModel{State = TournamentState.PostPlay});
        var tournamentBroadcastFake = new Mock<ITournamentBroadcast>();
        var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamentLogicFake.Object, tournamentBroadcastFake.Object);

        // Act
        var actionResult = tournamentController.AddTournamentStateBell(new Guid());

        // Assert
        tournamentRepositoryMock.Verify(mocks=>mocks.Update(It.IsAny<Tournament>()), Times.Once);
        }
        [Fact]
        public async void BroadcastTournamentStateChange_WhenStateChangesDueToBellCounter()
        {
        // Arrange
        var tournamentRepositoryFake = new Mock<ITournamentRepository>();
        var mapper = Helper.SetupMapper();
        var tournament = 
            new Tournament{ 
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1,
                BellCounter = 1,
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
                    }
                }
            };
        tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(tournament);
        var gameLogicFake = new Mock<IGameLogic>();        
        var tournamentLogicFake = new Mock<ITournamentLogic>();
        tournamentLogicFake.Setup(t=>t.AddBell(It.IsAny<Tournament>())).Returns(new TournamentStateUpdateModel{State = TournamentState.PrePlay});
        Game g = new Game();
        tournamentLogicFake.Setup(t=>t.SetPreplay(It.IsAny<Tournament>(), out g)).Returns(new Tournament{PreplayGameId = new Guid(),InplayGameId = null,PostplayGameId = null});
        var tournamentBroadcastMock = new Mock<ITournamentBroadcast>();
        var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamentLogicFake.Object, tournamentBroadcastMock.Object);

        // Act
        var actionResult = await tournamentController.AddTournamentStateBell(new Guid());

        // Assert
        tournamentBroadcastMock.Verify(mocks=>mocks.TournamentStateChangeAsync(It.IsAny<Guid>(), It.IsAny<TournamentState>(), It.IsAny<Guid?>(),It.IsAny<Guid?>(), It.IsAny<Guid?>()), Times.Once);
        }
    }
}