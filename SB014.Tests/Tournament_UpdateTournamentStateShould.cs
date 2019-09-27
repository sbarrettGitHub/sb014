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
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object);

            // Act 
            var actionResult = tournamentController.Update(Guid.NewGuid(), new JsonPatchDocument<Tournament>().Replace(t=>t.State,(int)TournamentStatus.PrePlay));

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
         [Fact]
         public void SetState_WhenStateChangeMade()
         {
            // Arrange
            Guid tId = new Guid();
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{Id=tId});
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper, gameLogicFake.Object);
            
            // Act
            var actionResult = tournamentController.Update(tId, new JsonPatchDocument<Tournament>().Replace(t=>t.State,(int)TournamentStatus.PrePlay));

            // Assert
            tournamentRepositoryMock.Verify(mock => mock.Update(It.Is<Tournament>(t=>t.State == (int)TournamentStatus.PrePlay)), Times.Once());
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