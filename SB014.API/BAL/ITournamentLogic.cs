using SB014.API.Domain;
using SB014.API.Models;

namespace  SB014.API.BAL
{
    public interface ITournamentLogic
    {
        Tournament SetPreplay(Tournament tournament, out Game newPreplayGame);
        Tournament SetInplay(Tournament tournament, out Game newPreplayGame);
        TournamentStateUpdateModel AddBell(Tournament tournament);
        Tournament SetPostplay(Tournament tournament, out Game newPostplayGame);
    }
}