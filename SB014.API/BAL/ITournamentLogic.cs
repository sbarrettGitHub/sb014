using SB014.API.Domain;

namespace  SB014.API.BAL
{
    public interface ITournamentLogic
    {
        Tournament SetPreplay(Tournament tournament, out Game newPreplayGame);
        Tournament SetInplay(Tournament tournament, out Game newPreplayGame);
    }
}