namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Enum;

//Figure out GameFormat which will be predefined, from GameType
// Changed from abstract class to regular class to support the InterestedGameTypes property in User
public class GameType : BaseEntity
{
    public required GameTypeSupported Name { get; set; }
    public List<Tournament>? Tournaments { get; set; }
}

