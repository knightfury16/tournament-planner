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
    private readonly ICurrentUser _currentUser;

    public AccountController(IMediator mediator, IIdentityService identityService, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _identityService = identityService;
        _currentUser = currentUser;
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
    [HttpGet("current-user")]
    public ActionResult GetCurrentUser()
    {
        return Ok(new { Email = _currentUser.Email, Name = _currentUser.Name, DomainId = _currentUser.DomainUserId });
    }
}