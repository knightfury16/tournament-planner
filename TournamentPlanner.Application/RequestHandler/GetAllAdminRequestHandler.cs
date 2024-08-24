using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAllAdminRequestHandler : IRequestHandler<GetAllAdminRequest, IEnumerable<AdminDto>>
{

    private readonly IRepository<Admin> _adminRepository;
    private readonly IMapper _mapper;

    public GetAllAdminRequestHandler(IRepository<Admin> adminRepository, IMapper mapper)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AdminDto>?> Handle(GetAllAdminRequest request, CancellationToken cancellationToken = default)
    {
        IEnumerable<Admin> admins;
        if (request.Name != null)
        {
            admins = await _adminRepository.GetAllAsync(admin => admin.Name.ToLowerInvariant().Contains(request.Name));
        }
        else
        {
            admins = await _adminRepository.GetAllAsync();
        }

        var adminsDtos = _mapper.Map<IEnumerable<AdminDto>>(admins);

        return adminsDtos;
    }
}


