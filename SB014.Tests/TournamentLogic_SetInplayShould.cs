using Xunit;
using SB014.API.Controllers;
using Moq;
using SB014.API.DAL;
using System;
using SB014.API.BAL;
using SB014.API.Domain;
using SB014.API.Domain.Enums;

namespace SB014.UnitTests.Api
{
    public class TournamentLogic_SetInplayShould
    {
         [Fact]
         public void BuildNewPreplayGame_WhenInPrePlay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicMock = new Mock<IGameLogic>();
            gameLogicMock.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicMock.Object);

            // Act
            Tournament t = tournamentLogic.SetInplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            gameLogicMock.Verify(mock => mock.BuildGame(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once());         
        }       
        [Fact]
        public void MoveExistingPreplayGameToInplay_WhenInPreplay()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
            Guid existingPreplayGameId = Guid.NewGuid();
            // Act
            Tournament t = tournamentLogic.SetInplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = existingPreplayGameId,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            Assert.True(t.InplayGameId == existingPreplayGameId);
        }
        [Fact]
        public void LeaveTournamentUnchanged_WhenInPostPlay()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);            
            
            // Act
            Tournament t = tournamentLogic.SetInplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = null,
                PostplayGameId = Guid.NewGuid(),
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Act + Assert
            Assert.True(t.State == TournamentState.PostPlay);
        }
        [Fact]
        public void LeaveTournamentUnchanged_WhenInInPlay()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);            
            
            // Act
            Tournament t = tournamentLogic.SetInplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = Guid.NewGuid(),
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Act + Assert
            Assert.True(t.State == TournamentState.InPlay);
        }
        [Fact]
        public void LeaveTournamentUnchanged_WhenInNoPlay()
        {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);            
            
            // Act
            Tournament t = tournamentLogic.SetInplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Act + Assert
            Assert.True(t.State == TournamentState.NoPlay);
        }
    }
}