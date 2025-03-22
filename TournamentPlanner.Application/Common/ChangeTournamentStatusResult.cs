namespace TournamentPlanner.Application.Common;

public class ChangeTournamentStatusResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; }

    private ChangeTournamentStatusResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static ChangeTournamentStatusResult Succeeded(
        string message = "Tournament status change successfully."
    )
    {
        return new ChangeTournamentStatusResult(true, message);
    }

    public static ChangeTournamentStatusResult Failed(
        string message = "Could not change tournament status."
    )
    {
        return new ChangeTournamentStatusResult(false, message);
    }
}
