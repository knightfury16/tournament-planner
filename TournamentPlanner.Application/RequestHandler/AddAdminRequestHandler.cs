using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class AddAdminRequestHandler : IRequestHandler<AddAdminRequest, AdminDto>
{
    public IRepository<Admin> _adminRepository { get; set; }
    public IMapper _mapper { get; set; }

    public AddAdminRequestHandler(IMapper mapper, IRepository<Admin> adminRepository)
    {
        _mapper = mapper;
        _adminRepository = adminRepository;
    }
    public async Task<AdminDto?> Handle(AddAdminRequest request, CancellationToken cancellationToken1 = default)
    {
        if(request == null){
            throw new ArgumentNullException(nameof(request));
        }
        var admin = _mapper.Map<Admin>(request.AddAdminDto);
        await _adminRepository.AddAsync(admin);
        await _adminRepository.SaveAsync();

        return _mapper.Map<AdminDto>(admin);
    }
}
