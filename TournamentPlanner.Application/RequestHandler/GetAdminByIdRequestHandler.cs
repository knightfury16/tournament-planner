using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminByIdRequestHandler: IRequestHandler<GetAdminByIdRequest, AdminDto>
{

    private readonly IRepository<Admin, Admin> _adminRepository;
    private readonly IMapper _mapper;

    public GetAdminByIdRequestHandler(IRepository<Admin, Admin> adminRepository, IMapper mapper)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
    }
    public async Task<AdminDto?> Handle(GetAdminByIdRequest request, CancellationToken cancellationToken1 = default)
    {

        var admin = await _adminRepository.GetAllAsync(a => a.Id == request.id, ["CreatedTournament"]);

        if (admin == null) return null;

        return _mapper.Map<AdminDto>(admin.First());

    }
}
