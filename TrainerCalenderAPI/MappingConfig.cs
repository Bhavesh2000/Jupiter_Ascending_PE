using AutoMapper;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;

namespace TrainerCalenderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                var mappingConfig = new MapperConfiguration(config => {
                    config.CreateMap<SessionDto, Session>();
                    config.CreateMap<Session, SessionDto>();
                });
            });
            return mappingConfig;
        }
    }
}
