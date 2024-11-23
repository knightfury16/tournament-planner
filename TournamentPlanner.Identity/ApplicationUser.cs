using Microsoft.AspNetCore.Identity;

namespace TournamentPlanner.Identity;

public class ApplicationUser : IdentityUser
{
    public int DomainUserId { get; set; }

}
