using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Constant;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class AddAdminRequestHandler : IRequestHandler<AddAdminRequest, AdminDto>
{
    public IRepository<Admin> _adminRepository { get; set; }
    private readonly IIdentityService _identityService;
    public IMapper _mapper { get; set; }

    public AddAdminRequestHandler(IMapper mapper, IRepository<Admin> adminRepository, IIdentityService identityService)
    {
        _mapper = mapper;
        _adminRepository = adminRepository;
        _identityService = identityService;
    }
    public async Task<AdminDto?> Handle(AddAdminRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        //tp admin
        Admin admin = await makeTpApplicationAdmin(request);

        //make identity application user
        await makeIdentityApplicationUser(admin, request.AddAdminDto.Password);

        return _mapper.Map<AdminDto>(admin);
    }

    private async Task makeIdentityApplicationUser(Admin admin, string password)
    {
        var applicationUserDto = new ApplicationUserDto
        {
            Email = admin.Email,
            Password = password,
            UserName = admin.Name,
            DomainUserId = admin.Id
        };
        try
        {
            var result = await _identityService.RegisterApplicationUserAndSigninAsync(applicationUserDto, true);
        }
        catch (System.Exception)
        {
            //remove the tp admin created
            await _adminRepository.DeleteByIdAsync(admin.Id);
            await _adminRepository.SaveAsync();
            throw;
        }

        //adding the Admin role
        await _identityService.AddRoleToApplicationUserAsync(admin.Email, Role.Admin);
    }

    private async Task<Admin> makeTpApplicationAdmin(AddAdminRequest request)
    {
        var admin = _mapper.Map<Admin>(request.AddAdminDto);
        var emailCheck = await _adminRepository.GetAllAsync(a => a.Email == request.AddAdminDto.Email);
        if (emailCheck.Any()) throw new BadRequestException("Email already exists");
        await _adminRepository.AddAsync(admin);
        await _adminRepository.SaveAsync();
        return admin;
    }
}
