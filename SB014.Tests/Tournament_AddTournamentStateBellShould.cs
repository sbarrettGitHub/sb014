using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SB014.API.BAL;
using SB014.API.Controllers;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Domain.Enums;
using SB014.API.Models;
using Xunit;

namespace SB014.UnitTests.Api
{
    public class Tournament_AddTournamentStateBellShould
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
            var actionResult = tournamentController.AddTournamentStateBell(new Guid());

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
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamentLogicMock.Object);
            
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
        tournamentLogicFake.Setup(t=>t.AddBell(It.IsAny<Tournament>())).Returns(new TournamentStateUpdateModel{State = TournamentState.NoPlay});
        var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object, tournamentLogicFake.Object);

        // Act
        var actionResult = tournamentController.AddTournamentStateBell(new Guid());

        // Assert
        tournamentRepositoryMock.Verify(mocks=>mocks.Update(It.IsAny<Tournament>()), Times.Once);
        }
    }
}