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
            // Set the state
            tournament.State = (int)Domain.Enums.TournamentStatus.PrePlay;
            
            // If Pre, In and Post play games are all un set then build a new game, set it as tournament preplay 
            // and save the game and tournament
            if(tournament.PreplayGameId.HasValue == false
                && tournament.InplayGameId.HasValue == false
                && tournament.PostplayGameId.HasValue == false)
                {
                    Game newPreplayGame = this.GameLogic.BuildGame(tournament.Id, tournament.CluesPerGame);
                    newPreplayGame.GameStatusId = (int)Domain.Enums.GameStatus.PrePlay;
                    this.TournamentRepository.UpdateGame(newPreplayGame);
                    
                    tournament.PreplayGameId = newPreplayGame.GameId;
                    this.TournamentRepository.Update(tournament);
                }
            
            return tournament;
        }
    }
}