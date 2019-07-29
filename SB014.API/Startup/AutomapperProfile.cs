using AutoMapper;
using SB014.Api.Entities;
using SB014.Api.Models;

namespace SB014.API
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Tournament, TournamentModel>();
            CreateMap<SubscribeToTournamentModel, Subscriber>();
            CreateMap<Subscriber,SubscriberModel>();
        }
    }
}