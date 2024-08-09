namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;

public abstract class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }

}
