using AutoMapper;
using SB014.API.Domain;
using SB014.API.Models;

namespace SB014.API
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Tournament, TournamentModel>();
            CreateMap<SubscribeToTournamentModel, Subscriber>();
            CreateMap<Subscriber,SubscriberModel>();
            CreateMap<Game,GameModel>();
            CreateMap<Clue,ClueModel>();
            CreateMap<AnswerAttempt,AnswerAttemptModel>();
            CreateMap<SubscriberGameResult,SubscriberGameResultModel>();

        }
    }
}