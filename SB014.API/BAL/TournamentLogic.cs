using System;
using System.Linq;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Models;

namespace SB014.API.BAL
{
    public class TournamentLogic : ITournamentLogic
    {
        private readonly IGameLogic GameLogic;
        private readonly ITournamentRepository TournamentRepository;

        public TournamentLogic(ITournamentRepository TournamentRepository, IGameLogic gameLogic)
        {
            this.TournamentRepository = TournamentRepository;
            this.GameLogic = gameLogic;
        }

        ///
        public Tournament SetPreplay(Tournament tournament, out Game newPreplayGame)
        {
            newPreplayGame = null;
            // Switch on the current state
            switch (tournament.State)
            {
                case Domain.Enums.TournamentState.NoPlay:
                    // Create a new preplay game
                    newPreplayGame = this.GameLogic.BuildGame(tournament.Id, tournament.CluesPerGame);                    
                    
                    tournament.PreplayGameId = newPreplayGame.Id;
                    tournament.InplayGameId = null;
                    tournament.PostplayGameId = null;
                    
                break;

                case Domain.Enums.TournamentState.InPlay:
                    newPreplayGame = null;
                    
                    // Set post play game to curent inplay game
                    tournament.PostplayGameId = tournament.InplayGameId;
                    
                    // Clear the inplay game
                    tournament.InplayGameId = null;

                break;
                case Domain.Enums.TournamentState.PrePlay:
                    
                break;
                case Domain.Enums.TournamentState.PostPlay:
                    // Clear the post play game
                    tournament.PostplayGameId = null;
                break;
            }          

            return tournament;
        }
        public Tournament SetInplay(Tournament tournament, out Game newPreplayGame)
        {
            newPreplayGame = null;
            // Switch on the current state
            switch (tournament.State)
            {
                case Domain.Enums.TournamentState.NoPlay:
                    
                break;

                case Domain.Enums.TournamentState.InPlay:
                    

                break;
                case Domain.Enums.TournamentState.PrePlay:
                    // Create a new preplay game
                    newPreplayGame = this.GameLogic.BuildGame(tournament.Id, tournament.CluesPerGame);   
                    
                    // Clear post play
                    tournament.PostplayGameId = null;

                    // Move PrePlay to Inplay
                    tournament.InplayGameId = tournament.PreplayGameId;

                    // New preplay
                    tournament.PreplayGameId = newPreplayGame.Id;
                    
                    
                break;
                case Domain.Enums.TournamentState.PostPlay:
                    break;
            }
            return tournament;
        }    
        public Tournament SetPostplay(Tournament tournament, out Game newPostplayGame)
        {
            newPostplayGame = null;
            // Switch on the current state
            switch (tournament.State)
            {
                case Domain.Enums.TournamentState.NoPlay:
                    
                break;

                case Domain.Enums.TournamentState.InPlay:

                    // Move inplay to Postplay
                    tournament.PostplayGameId = tournament.InplayGameId;

                    // Clear inplay game
                    tournament.InplayGameId = null;
                    
                break;
                case Domain.Enums.TournamentState.PrePlay:
                    // Create a new postplay game
                    newPostplayGame = this.GameLogic.BuildGame(tournament.Id, tournament.CluesPerGame);                      
                    
                    // New Postplay
                    tournament.PostplayGameId = newPostplayGame.Id;
                                        
                break;
                case Domain.Enums.TournamentState.PostPlay:
                    break;
            }
            return tournament;
        }      
        public TournamentStateUpdateModel AddBell(Tournament tournament)
        {
            tournament.BellCounter ++;

            // Get the state associated with the current bell counter from the Bell State Lookup Matrix
            BellStateLookup bellStateLookup = (from m in tournament.BellStateLookupMatrix 
                        where m.BellCounter == tournament.BellCounter
                        select m).FirstOrDefault();
            
            // Reset bell counter to the initial  state bell counter if bell counter has no matching state
            if(bellStateLookup == null){
                int initialLookupBellCounter = tournament.BellStateLookupMatrix.Min(entry => entry.BellCounter);
                bellStateLookup = tournament.BellStateLookupMatrix.Where(entry => entry.BellCounter == initialLookupBellCounter).FirstOrDefault();
                if(bellStateLookup == null)
                {
                    throw new System.Exception("Cannot determine tournament initial state");
                }
                //Reset the bell counter
                tournament.BellCounter = bellStateLookup.BellCounter;
            }
            return new TournamentStateUpdateModel{State = bellStateLookup.State};
        }
    }
}