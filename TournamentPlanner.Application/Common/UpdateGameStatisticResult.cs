namespace TournamentPlanner.Application.Common;

public class UpdateGameStatisticResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; }

    private UpdateGameStatisticResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static UpdateGameStatisticResult Succeeded(
        string message = "Game Statistic updated successfully."
    )
    {
        return new UpdateGameStatisticResult(true, message);
    }

    public static UpdateGameStatisticResult Failed(string message = "Game Statistic update failed.")
    {
        return new UpdateGameStatisticResult(false, message);
    }
}
