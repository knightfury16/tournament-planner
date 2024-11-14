using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Application.GameTypeScore;

public class EightBallPoolScore : IScore
{
    public int Player1Racks { get; set; }
    public int Player2Racks { get; set; }
    public int RaceTo { get; set; } = 5; //default 5
    public bool IsComplete =>  Player1Racks == RaceTo || Player2Racks == RaceTo;
}
