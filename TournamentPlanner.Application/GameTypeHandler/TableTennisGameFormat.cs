using System.Text.Json;
using TournamentPlanner.Application.GameTypeScore;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Application.GameTypeHandler;

//TODO: refactor it with proper error message
public class TableTennisGameFormat : GameFormat
{

    public override IScore DeserializeScore(object scoreData)
    {
        string? scoreString = scoreData.ToString();

        if (scoreString == null)
        {
            throw new Exception("Can not convert object to string");
        }
        TableTennisScore? tableTennisScore = JsonSerializer.Deserialize<TableTennisScore>(scoreString, JsonOptions);
        if (tableTennisScore == null)
        {
            throw new JsonException($"Error Deserializing {nameof(TableTennisScore)}");
        }
        return tableTennisScore;
    }

    public override Player DetermineWinner(Player player1, Player player2, IScore score)
    {
        var tabbleTennisScore = (TableTennisScore)score;
        return tabbleTennisScore.Player1Sets > tabbleTennisScore.Player2Sets ? player1 : player2;
    }

    public override bool IsValidScore(IScore score)
    {
        var tableTennisScore = (TableTennisScore)score;

        if (tableTennisScore == null)
        {
            return false;
        }
        if (tableTennisScore.Player1Sets + tableTennisScore.Player2Sets > tableTennisScore.SetsToWin * 2 - 1)
            return false;

        foreach (var setScore in tableTennisScore.SetScores)
        {
            if (setScore.Player1Points < tableTennisScore.PointsPerSet && setScore.Player2Points < tableTennisScore.PointsPerSet)
                return false;
            if (Math.Abs(setScore.Player1Points - setScore.Player2Points) < 2)
                return false;
        }

        return true;
    }

    public override IScore CreateInitialScore()
    {
        return new TableTennisScore();
    }
}

