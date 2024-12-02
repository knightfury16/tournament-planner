using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Constant;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Identity.Model;

namespace TournamentPlanner.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private bool lockoutOnFailure = false;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    public async Task<bool> RegisterApplicationUserAndSigninAsync(ApplicationUserDto applicationUserDto, bool persistent = false)
    {
        var user = new ApplicationUser
        {
            Email = applicationUserDto.Email,
            UserName = applicationUserDto.UserName,
            PhoneNumber = applicationUserDto.PhoneNumber,
            DomainUserId = applicationUserDto.DomainUserId
        };

        var result = await _userManager.CreateAsync(user, applicationUserDto.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create user, errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await AddDefaultApplicationUserCalims(applicationUserDto);

        await _signInManager.SignInAsync(user, isPersistent: persistent);

        return result.Succeeded;
    }


    public async Task<bool> LoginApplicationUserAsync(ApplicationUserDto applicationUserDto, bool persistent = false)
    {
        var user = await _userManager.FindByEmailAsync(applicationUserDto.Email);
        if (user == null)
        {
            throw new NotFoundException("User", applicationUserDto.Email);
        }

        var result = await _signInManager.PasswordSignInAsync(user, applicationUserDto.Password, persistent, lockoutOnFailure);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        await _signInManager.SignInAsync(user, isPersistent: persistent);

        return result.Succeeded;
    }

    public async Task AddRoleToApplicationUserAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) throw new NotFoundException(nameof(user));

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists) throw new ValidationException($"Role '{roleName}' does not exist.");

        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded;
    }



    public async Task<bool> RegisterApplicationUserAsync(ApplicationUserDto applicationUserDto)
    {
        var user = new ApplicationUser
        {
            Email = applicationUserDto.Email,
            UserName = applicationUserDto.UserName,
            PhoneNumber = applicationUserDto.PhoneNumber,
            DomainUserId = applicationUserDto.DomainUserId
        };

        var result = await _userManager.CreateAsync(user, applicationUserDto.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create user, errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await AddDefaultApplicationUserCalims(applicationUserDto);

        return result.Succeeded;
    }

    public async Task SignoutApplicationUserAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task AddClaimToApplicationUserAsync(string email, string claimType, string claimValue)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new NotFoundException(nameof(user));

        var claim = new Claim(claimType, claimValue);
        await _userManager.AddClaimAsync(user, claim);
    }

    public async Task<bool> CheckUserClaimAsync(string email, string claimType, string claimValue)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new NotFoundException(nameof(user));

        var userClaims = await _userManager.GetClaimsAsync(user);
        return userClaims.Any(claim => claim.Type == claimType && claim.Value == claimValue);
    }

    public async Task<List<Claim>> GetAllClaimsOfApplicationUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new NotFoundException(nameof(user));

        var userClaims = await _userManager.GetClaimsAsync(user);
        return userClaims.ToList();
    }

    public Task<List<Claim>> GetAllClaimsOfCurrentApplicationUser()
    {
        throw new NotImplementedException();
    }
    public async Task<List<Claim>> GetAllClaimsOfApplicationUser(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null) throw new NotFoundException(nameof(user));

        var userClaims = await _userManager.GetClaimsAsync(user);
        return userClaims.ToList();
    }
    private async Task AddDefaultApplicationUserCalims(ApplicationUserDto applicationUserDto)
    {
        await AddClaimToApplicationUserAsync(applicationUserDto.Email, ClaimTypes.Email, applicationUserDto.Email);
        await AddClaimToApplicationUserAsync(applicationUserDto.Email, ClaimTypes.Name, applicationUserDto.UserName!);
        if (applicationUserDto.DomainUserId > 0) await AddClaimToApplicationUserAsync(applicationUserDto.Email, DomainClaim.DomainUserIdClaimType, applicationUserDto.DomainUserId.ToString());
    }

    public async Task<List<string>> GetAllRolesOfUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new NotFoundException(nameof(user));

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<List<Claim>> GetRoleClaimsOfuser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception(nameof(user));

        var userRoles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in userRoles)
        {
            var identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole == null) continue;
            var claims = await _roleManager.GetClaimsAsync(identityRole);
            roleClaims.AddRange(claims);
        }

        return roleClaims;
    }
}
