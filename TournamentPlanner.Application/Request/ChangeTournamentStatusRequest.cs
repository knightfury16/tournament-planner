using TournamentPlanner.Application.Common;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class ChangeTournamentStatusRequest : IRequest<ChangeTournamentStatusResult>
{
    public TournamentStatus TournamentStatus { get; set; }
    public int TournamentId { get; set; }

    public ChangeTournamentStatusRequest(int tournamentId, string tournamentStatus)
    {
        TournamentId = tournamentId;

        TournamentStatus = GetTournamentStatusEnum(tournamentStatus);
    }

    public TournamentStatus GetTournamentStatusEnum(string statusString)
    {
        var trimString = GetTrimmedString(statusString);

        if (!Enum.TryParse(trimString, out TournamentStatus tournamentStatus))
        {
            return TournamentStatus.Draft;
        }

        return tournamentStatus;
    }

    private string GetTrimmedString(string statusString)
    {
        statusString = statusString.Replace(" ", "");
        return statusString.Trim();
    }
}
