
using AutoMapper;
using SB014.API;

namespace SB014.UnitTests.Api
{
    public class Helper
    {
            internal static IMapper SetupMapper()
            {
                var profile = new AutomapperProfile();  
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
                return new Mapper(configuration);          
            }

    }
}