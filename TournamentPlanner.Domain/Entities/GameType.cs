using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities
{
    //Figure out GameFormat which will be predefined, from GameType
    // Changed from abstract class to regular class to support the InterestedGameTypes property in User
    public class GameType : BaseEntity
    {
        //TODO: Make a enum of the supported GameType
        public required string Name { get; set; }

    }

}