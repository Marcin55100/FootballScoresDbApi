using AutoMapper;
using FootballScoresDbApi.Models;
using FootballScoresDbApi.Models.DTOs;

namespace FootballScoresDbApi.Mapper
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
