using System;

namespace SB014.API.DAL
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public Guid? GameId { get; set; }
        public int? GameStatusId { get; set; }

        public int CluesPerGame { get; set; }
    }


}