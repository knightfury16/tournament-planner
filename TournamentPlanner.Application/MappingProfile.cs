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
            CreateMap<GameType, GameTypeDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
            .ReverseMap();
            CreateMap<Tournament, TournamentDto>()
            .ForMember(dest => dest.GameTypeDto, opt => opt.MapFrom(src => src.GameType))
            .ReverseMap();
            CreateMap<Tournament, FullTournamentDto>()
            .ForMember(dest => dest.GameTypeDto, opt => opt.MapFrom(src => src.GameType))
            .ReverseMap();
            CreateMap<Player, AddPlayerDto>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<Admin, AddAdminDto>().ReverseMap();
            CreateMap<Tournament, AddTournamentDto>().ReverseMap();
            CreateMap<Match, MatchDto>().ReverseMap();
            CreateMap<Domain.Entities.MatchType, MatchTypeDto>().ReverseMap();
            CreateMap<Draw, DrawDto>().ReverseMap();

        }

    }
}