using System.Text.Json;
using TournamentPlanner.Application.Common.Interfaces;

namespace TournamentPlanner.Application.GameTypeHandler;

public class TableTennisGameTypeHandler : IGameTypeHandler
{
    public IScore DeserializeScore(string scoreData)
    {
        var tabbleTennisScoreFormat = JsonSerializer.Deserialize<TableTennisScoreFormat>(scoreData);
        if (tabbleTennisScoreFormat == null)
        {
            throw new JsonException($"Error Desirializing {nameof(TableTennisScoreFormat)}");
        }
        return tabbleTennisScoreFormat;
    }

    public string SerializeScore(IScore score)
    {
        return JsonSerializer.Serialize(score);
    }

    public bool ValidateGameSpecificData(string gameSpecificScore)
    {
        TableTennisScoreFormat tableTennisScoreFormat = (TableTennisScoreFormat)DeserializeScore(gameSpecificScore);

        if (tableTennisScoreFormat == null) return false;

        foreach (var setScore in tableTennisScoreFormat.SetScores)
        {
            //check if the differenve between setscore is at least two or not
            if (Math.Abs(setScore.Player1Points - setScore.Player2Points) < 2)
            {
                return false;
            }

        }
        return true;
    }
}


public interface IScore
{

}

public class TableTennisScoreFormat : IScore
{
    public int Player1Sets { get; set; }
    public int Player2Sets { get; set; }
    public List<(int Player1Points, int Player2Points)> SetScores { get; set; } = new List<(int, int)>();
}