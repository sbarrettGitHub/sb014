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
         public void BuildNewPreplayGame_WhenAllGamesAreUnSet()
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
            });

            // Assert
            gameLogicMock.Verify(mock => mock.BuildGame(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once());         
        }
        [Fact]
         public void SaveNewPreplayGame_WhenAllAreUnSet()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
 
            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            });

            // Assert
            tournamentRepositoryMock.Verify(mock => mock.UpdateGame(It.IsAny<Game>()), Times.Once());      
        }
        [Fact]
         public void UpdateTournamentPreplayGameId_WhenAllGamesAreUnSetAndNewPreplayGameCreated()
         {
            // Arrange
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            Guid preplayGameId = new Guid();
            gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game{GameId = preplayGameId});
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
 
            // Act
            Tournament t = tournamentLogic.SetPreplay(new Tournament{
                Id = new Guid(),
                PreplayGameId = null,
                InplayGameId = null,
                PostplayGameId = null,
                CluesPerGame = 1
            });

            // Assert
            tournamentRepositoryMock.Verify(mock => mock.Update(It.Is<Tournament>(x=>x.PreplayGameId == preplayGameId)), Times.Once());      
        }
  
        [Fact]
        public void MoveExistingInplayGameToPostplay_WhenInplay()
        {
        //  // Arrange
        //     var tournamentRepositoryMock = new Mock<ITournamentRepository>();
        //     var mapper = Helper.SetupMapper();
        //     var gameLogicFake = new Mock<IGameLogic>();
        //     gameLogicFake.Setup(m=>m.BuildGame(It.IsAny<Guid>(),It.IsAny<int>())).Returns(new Game());
        //     ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryMock.Object, gameLogicFake.Object);
        //     Guid existingInplayGameId = Guid.NewGuid();
        //     // Act
        //     Tournament t = tournamentLogic.SetPreplay(new Tournament{
        //         Id = new Guid(),
        //         PreplayGameId = Guid.NewGuid(),
        //         InplayGameId = existingInplayGameId,
        //         PostplayGameId = null,
        //         CluesPerGame = 1,
        //         State = (int)TournamentStatus.InPlay
        //     });

        //     // Assert
        //     tournamentRepositoryMock.Verify(mock => mock.Update(It.Is<Tournament>(x=>x.PostplayGameId == existingInplayGameId)), Times.Once());             
        }
        
        [Fact]
        public void BuildNewPreplayGame_WhenInplay()
        {
                 
        }  
        [Fact]
        public void TestName()
        {
        //Given
        
        //When
        
        //Then
        }
    }
}