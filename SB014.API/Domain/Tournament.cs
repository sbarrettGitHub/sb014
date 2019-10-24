using System;
using System.Collections.Generic;
using SB014.API.Domain.Enums;

namespace SB014.API.Domain
{
    public class BellStateLookup
    {
        public int BellCounter { get; set; }
        public TournamentState State { get; set; }
    }
    public class Tournament
    {
        public Guid Id { get; set; }
        public Guid? PreplayGameId { get; set; }
        public Guid? InplayGameId { get; set; }
        public Guid? PostplayGameId { get; set; }

        public int CluesPerGame { get; set; }
        public List<BellStateLookup> BellStateLookupMatrix { get; set; }
        public TournamentState State {
            get{
                if(this.PreplayGameId.HasValue == false
                    && this.InplayGameId.HasValue == false
                    && this.PostplayGameId.HasValue == false)
                {
                    return TournamentState.NoPlay;
                }
                else if(this.PreplayGameId.HasValue && this.InplayGameId.HasValue)
                {
                    return TournamentState.InPlay;
                }
                else if(this.PreplayGameId.HasValue && this.InplayGameId.HasValue == false && this.PostplayGameId.HasValue)
                {
                    return TournamentState.PostPlay;
                }
                else
                {
                    return TournamentState.PrePlay;
                }
            }
        }

        public int BellCounter { get; set; }
    }


}