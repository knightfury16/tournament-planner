using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class AddPlayerRequestHandler : IRequestHandler<AddPlayerRequest, PlayerDto>
    {
        public IRepository<Player> _playerRepository { get; set; }
        public IMapper _mapper { get; set; }

        public AddPlayerRequestHandler(IRepository<Player> playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }
        public async Task<PlayerDto?> Handle(AddPlayerRequest request, CancellationToken cancellationToken1 = default)
        {
            var player = _mapper.Map<Player>(request.AddPlayerDto);

            await _playerRepository.AddAsync(player);
            await _playerRepository.SaveAsync();

            return _mapper.Map<PlayerDto>(player);
        }
    }
}