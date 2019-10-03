using SB014.API.DAL;
using SB014.API.Domain;
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
            }
            return tournament;
        }
    }
}