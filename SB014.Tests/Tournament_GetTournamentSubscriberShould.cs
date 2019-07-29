using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SB014.Api.Models;
using SB014.Api.Repository;
using Moq;
using SB014.Api.Entities;
using System;

namespace SB014.UnitTests.Api
{
    public class Tournament_GetTournamentSubscriberShould
    {
        [Fact]
        public void ReturnStatusOK_WhenSubscriberResourceExists()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            // Act 
            var  actionResult = tournamentController.GetTournamentSubscriber(new Guid(), new Guid());

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenTournamentResourceDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns<Tournament>(null);
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new Subscriber());
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            // Act 
            var  actionResult = tournamentController.GetTournamentSubscriber(new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusNotFound_WhenSubscriberResourceDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.GetSubscriber(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns<Subscriber>(null);
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            // Act 
            var  actionResult = tournamentController.GetTournamentSubscriber(new Guid(), new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}