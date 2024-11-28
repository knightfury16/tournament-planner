using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Api.Controllers;

[ApiController]
[Route("/api/identity/account")]
public class AccountController : ControllerBase
{
    public IMediator _mediator { get; }
    private readonly IIdentityService _identityService;

    public AccountController(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRole()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email == null) return BadRequest("User email claim could not be found");
        var roles = await _identityService.GetAllRolesOfUser(email);
        return Ok(roles);
    }

    [HttpGet("role-claims")]
    public async Task<ActionResult<IEnumerable<Claim>>> GetRoleClaims()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email == null) return BadRequest("User email claim could not be found");
        var roleClaims = await _identityService.GetRoleClaimsOfuser(email);
        return Ok(roleClaims);
    }

    [HttpGet("claims")]
    public async Task<ActionResult<IEnumerable<Claim>>> GetUserClaims()
    {
        var claims = await _identityService.GetAllClaimsOfApplicationUser(User);
        return Ok(claims);
    }
}