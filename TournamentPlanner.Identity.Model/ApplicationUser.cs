namespace TournamentPlanner.Identity.Model;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public int DomainUserId { get; set; }
}