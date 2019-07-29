using Xunit;
using SB014.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SB014.Api.Repository;
using Moq;
using SB014.Api.Entities;
using System;
using SB014.Api.Models;

namespace SB014.UnitTests.Api
{
    public class Tournament_SubscribeToTournamentShould
    {
        [Fact]
        public void ReturnStatusNotFound_WhenTournamentDoesNotExist()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns<Tournament>(null);
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);

            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public void ReturnStatusBadRequest_WhenInsufficientDataSupplied()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);
            tournamentController.ModelState.AddModelError("fakeError", "fakeError");
            
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), null);
            
            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public void AddSubscriberToRepository()
        {
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryMock.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryMock.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var tournamentController = new TournamentController(tournamentRepositoryMock.Object, mapper);
    
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            tournamentRepositoryMock.Verify(mock => mock.AddSubscriber(It.IsAny<Subscriber>()), Times.Once());
            
        }
        [Fact]

        public void ReturnSubscriberCreatedAtRoute()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            tournamentRepositoryFake.Setup(p=>p.Get(It.IsAny<Guid>())).Returns(new Tournament());
            tournamentRepositoryFake.Setup(p=>p.AddSubscriber(It.IsAny<Subscriber>())).Returns(new Subscriber {Id = new Guid()});
            var tournamentController = new TournamentController(tournamentRepositoryFake.Object, mapper);
     
            // Act 
            var  actionResult = tournamentController.SubscribeToTournament(Guid.NewGuid(), new SubscribeToTournamentModel{Name="test"});

            // Assert
            Assert.IsType<CreatedAtRouteResult>(actionResult); 
            
        }
    }
}