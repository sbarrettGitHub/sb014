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
        public Tournament SetPreplay(Tournament tournament)
        {
            switch (tournament.State)
            {
                case Domain.Enums.TournamentState.NoPlay:
                    // Set tournament as  preplay 
                    // and save the game and tournament
                    Game newPreplayGame = this.GameLogic.BuildGame(tournament.Id, tournament.CluesPerGame);
                    this.TournamentRepository.UpdateGame(newPreplayGame);
                    
                    tournament.PreplayGameId = newPreplayGame.GameId;
                    tournament.InplayGameId = null;
                    tournament.PostplayGameId = null;
                    this.TournamentRepository.Update(tournament);
                break;
                case Domain.Enums.TournamentState.InPlay:
                break;
                case Domain.Enums.TournamentState.PrePlay:
                break;
            }          

            return tournament;
        }
    }
}