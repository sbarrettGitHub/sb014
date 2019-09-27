using System.Collections.Generic;

namespace SB014.API.Models
{
    public class GameModel
    {
        public int GameStatusId {get; set;}
        public List<ClueModel> Clues { get; set; }
    }
}