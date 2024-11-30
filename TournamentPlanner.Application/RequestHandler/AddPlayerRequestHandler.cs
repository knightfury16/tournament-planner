using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Constant;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class AddPlayerRequestHandler : IRequestHandler<AddPlayerRequest, PlayerDto>
    {
        public IRepository<Player> _playerRepository { get; set; }
        private readonly IIdentityService _identityService;
        public IMapper _mapper { get; set; }

        public AddPlayerRequestHandler(IRepository<Player> playerRepository, IMapper mapper, IIdentityService identityService)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<PlayerDto?> Handle(AddPlayerRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            Player player = await makeTpApplicationPlayer(request);
            await makeIdentityApplicationUser(player, request.AddPlayerDto.Password);

            return _mapper.Map<PlayerDto>(player);
        }

        private async Task makeIdentityApplicationUser(Player player, string password)
        {
            var applicationUserDto = new ApplicationUserDto
            {
                Email = player.Email,
                Password = password,
                UserName = player.Name,
                DomainUserId = player.Id
            };
            var result = await _identityService.RegisterApplicationUserAndSigninAsync(applicationUserDto, true);
            if (!result)
            {
                //remove the tp admin created
                await _playerRepository.DeleteByIdAsync(player.Id);
                await _playerRepository.SaveAsync();
                throw new InternalServerErrorException("Could not create identity user.");
            }

            await _identityService.AddRoleToApplicationUserAsync(player.Email, Role.Player);
        }

        private async Task<Player> makeTpApplicationPlayer(AddPlayerRequest request)
        {
            var player = _mapper.Map<Player>(request.AddPlayerDto);

            await _playerRepository.AddAsync(player);
            await _playerRepository.SaveAsync();
            return player;
        }
    }
}