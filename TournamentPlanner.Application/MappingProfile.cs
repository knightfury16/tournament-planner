using AutoMapper;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player,PlayerDto>().ReverseMap();
            CreateMap<Player,FullPlayerDto>().ReverseMap();
        }
        
    }
}