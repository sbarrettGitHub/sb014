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
    public class TournamentLogic_SetPreplayShould
    {

         [Fact]
         public void BuildNewPreplayGame_WhenInNoPlay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicMock = new Mock<IGameLogic>();
            gameLogicMock.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicMock.Object);

            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            gameLogicMock.Verify(mock => mock.BuildGame(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once());         
        }       
 

        [Fact]
         public void SetPreplayGame_WhenInNoPlayAndNewPreplayGameCreated()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            Guid newPreplayGameId = new Guid();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game{Id = newPreplayGameId});
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
 
            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            Assert.Equal(t.PreplayGameId, newPreplayGameId );
        }
  
        [Fact]
        public void MoveExistingInplayGameToPostplay_WhenInplay()
        {
         // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
            Guid existingInplayGameId = Guid.NewGuid();
            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = existingInplayGameId,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            Assert.True(t.PostplayGameId == existingInplayGameId);
        }

        [Fact]
        public void ClearExistingInplayGame_WhenInplay()
        {
         // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
            Guid existingInplayGameId = Guid.NewGuid();
            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = Guid.NewGuid(),
                InplayGameId = existingInplayGameId,
                PostplayGameId = null,
                CluesPerGame = 1
            },out Game newPreplayGame);

            // Assert
            Assert.True(t.InplayGameId == null);
        }        
    }
}