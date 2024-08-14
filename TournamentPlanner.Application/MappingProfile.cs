using AutoMapper;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>().ReverseMap();
            CreateMap<Player, FullPlayerDto>().ReverseMap();
            CreateMap<Tournament, TournamentDto>().ReverseMap();
            CreateMap<Player, AddPlayerDto>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<Admin, AddAdminDto>().ReverseMap();
        }

    }
}