using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SB014.API.Models;
using Moq;
using SB014.API.DAL;
using SB014.API.BAL;
using SB014.API.Domain;
using System;

namespace SB014.UnitTests.Api
{
    public class Tournament_GetTournamentShould
    {
        [Fact]
        public void ReturnStatusNotFound_WhenTournamentDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns<Tournament>(null);
            tournamentRepositoryFake.Setup(p=>p.GetGame(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns<Game>(null);
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            // Act 
            var actionResult = tournamentController.GetTournament(new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnTournamnet_WhenTournamentExists()
        {
            //Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
          
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            var gameLogicFake = new Mock<IGameLogic>();
            var tournamnetLogicFake = new Mock<ITournamentLogic>();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper, gameLogicFake.Object, tournamnetLogicFake.Object);

            //Act
            var  actionResult = tournamentController.GetTournament(Guid.NewGuid());
            var contentResult = actionResult as OkObjectResult; 
            var tournament =  (TournamentModel)contentResult.Value;

            // Assert
            Assert.IsType<TournamentModel>(contentResult.Value);

        }
    }
}