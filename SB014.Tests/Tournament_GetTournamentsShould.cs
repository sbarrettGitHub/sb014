using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SB014.API.Models;
using SB014.API.Repository;
using Moq;
using SB014.API.Entities;

namespace SB014.UnitTests.Api
{
    public class Tournament_GetTournamentsShould
    {
       
        [Fact]
        public void ReturnStatusOK_WhenTournamentResourceExists()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.GetAll()).Returns(new List<Tournament>());
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            // Act 
            var  actionResult = tournamentController.GetTournaments();

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void ReturnEmptyListOftournaments_WhenNoneExist()
        {
            //Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
          
            tournamentRepositoryFake.Setup(p=>p.GetAll()).Returns(new List<Tournament>());
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            //Act
            var  actionResult = tournamentController.GetTournaments();
            var contentResult = actionResult as OkObjectResult; 
            var tournamentsList =  (List<TournamentModel>)contentResult.Value;

            //Assert
            Assert.Empty(tournamentsList);

        }
        [Fact]
        public void ReturnPopulatedListOfTournaments_WhenTournamentsExist()
        {
            //Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.GetAll()).Returns(new List<Tournament>{new Tournament()});
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            //Act
            var  actionResult = tournamentController.GetTournaments();
            var contentResult = actionResult as OkObjectResult; 
            List<TournamentModel> tournamentsList =  (List<TournamentModel>)contentResult.Value;

            //Assert
            Assert.NotEmpty(tournamentsList);

        }

    }   
}