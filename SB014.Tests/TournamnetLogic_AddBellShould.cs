using System;
using System.Collections.Generic;
using Moq;
using SB014.API.BAL;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Domain.Enums;
using Xunit;

namespace SB014.UnitTests.Api
{
    public class TournamentLogic_AddBellShould
    {
        [Fact]
        public void SetTournamentBellCounter()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicFake.Object);
            Tournament tournament = new Tournament{
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
                        State = TournamentState.PrePlay
                    },
                    new BellStateLookup
                    {
                        BellCounter = 3,
                        State = TournamentState.InPlay
                    }
                }};
            
            // Act
            tournamentLogic.AddBell(tournament);

            // Assert
            Assert.NotEqual(0,tournament.BellCounter);
        }
        [Fact]
        public void SaveTournament_WhenStateChangesDueToBellCounter()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicFake.Object);
            Tournament tournament = new Tournament{
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
                        State = TournamentState.PrePlay
                    },
                    new BellStateLookup
                    {
                        BellCounter = 3,
                        State = TournamentState.InPlay
                    }
                }};
            
            // Act
            tournamentLogic.AddBell(tournament);

            // Assert
            Assert.NotEqual(0,tournament.BellCounter);
        }        
        [Fact]
        public void DetermineNewTournamentState_WhenBellStateMatrixAvailable()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicFake.Object);
            Tournament tournament = new Tournament{
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
                        State = TournamentState.PrePlay
                    },
                    new BellStateLookup
                    {
                        BellCounter = 3,
                        State = TournamentState.InPlay
                    }
                }
            };
            // Act
            var newState = tournamentLogic.AddBell(tournament);

            // Assert
            Assert.Equal( TournamentState.PrePlay, newState.State);

        }
        [Fact]
        public void ResetBellCounterToInitialState_WhenNoBellStateDefinedForBellCounter()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicFake.Object);
            Tournament tournament = new Tournament{
                BellCounter = 100,
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
                        State = TournamentState.PrePlay
                    },
                    new BellStateLookup
                    {
                        BellCounter = 3,
                        State = TournamentState.InPlay
                    }
                }
            };
        
            // Act
            var newState = tournamentLogic.AddBell(tournament);
        
            // Assert
            //Assert.Equal( TournamentState.PrePlay, newState.State);
            Assert.Equal( 1, tournament.BellCounter);
        }
        [Fact]
        public void ReturnInitialState_WhenNoBellStateDefinedForBellCounter()
        {
            // Arrange
            var tournamentRepositoryFake = new Mock<ITournamentRepository>();
            var mapper = Helper.SetupMapper();
            var gameLogicFake = new Mock<IGameLogic>();
            ITournamentLogic tournamentLogic = new TournamentLogic(tournamentRepositoryFake.Object, gameLogicFake.Object);
            Tournament tournament = new Tournament{
                BellCounter = 100,
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
        
            // Act
            var newState = tournamentLogic.AddBell(tournament);
        
            // Assert
            Assert.Equal( TournamentState.PrePlay, newState.State);
        }    
    }
}