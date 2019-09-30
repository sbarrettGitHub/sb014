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
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.Update(Guid.NewGuid(), new JsonPatchDocument<Tournament>().Replace(t=>t.State,(int)TournamentStatus.PrePlay));

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

         [Fact]
         public void ApplyTournamentPreplayLogicRules_WhenStateChangeMadeFromNoplayToPreplay()
         {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament{State = (int)TournamentStatus.NoPlay});
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicMock = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicMock.Object);
            
            // Act
            var actionResult = tournamentController.Update(new Guid(), new JsonPatchDocument<Tournament>().Replace(t=>t.State,(int)TournamentStatus.PrePlay));

            tournamnetLogicMock.Verify(mocks=>mocks.SetPreplay(It.IsAny<Tournament>()), Times.Once);
         }
    }
}