using System.Text.Json;
using TournamentPlanner.Application.GameTypeScore;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Application.GameTypeHandler;

public class EightBallPoolGameFormat : GameFormat
{
    public override IScore CreateInitialScore()
    {
        return new EightBallPoolScore();
    }

    public override IScore DeserializeScore(object scoreData)
    {
        string? scoreString = scoreData.ToString();

        if (scoreString == null)
        {
            throw new Exception("Can not convert object to string");
        }
        EightBallPoolScore? eightBallPoolScore = JsonSerializer.Deserialize<EightBallPoolScore>(
            scoreString,
            JsonOptions
        );
        if (eightBallPoolScore == null)
        {
            throw new JsonException($"Error Deserializing {nameof(EightBallPoolScore)}");
        }
        return eightBallPoolScore;
    }

    public override Player DetermineWinner(Player player1, Player player2, IScore score)
    {
        var eightBallPoolScore = (EightBallPoolScore)score;
        return eightBallPoolScore.Player1Racks == eightBallPoolScore.RaceTo ? player1 : player2;
    }

    public override bool IsValidScore(IScore score)
    {
        var eightBallPoolScore = (EightBallPoolScore)score;

        if (eightBallPoolScore == null)
        {
            return false;
        }
        if (
            eightBallPoolScore.Player1Racks + eightBallPoolScore.Player2Racks
            > eightBallPoolScore.RaceTo * 2 - 1
        )
            return false;

        // If either player not yet reached race to value thats why and
        if (
            eightBallPoolScore.Player1Racks != eightBallPoolScore.RaceTo
            && eightBallPoolScore.Player2Racks != eightBallPoolScore.RaceTo
        )
            return false;

        return true;
    }

    public override List<PlayerStanding> GetGroupStanding(
        Tournament tournament,
        Domain.Entities.MatchType matchType,
        bool completeStanding = false
    )
    {
        if (tournament == null || matchType == null)
            throw new ArgumentNullException(nameof(GetGroupStanding));

        if (matchType.Rounds.Count == 0)
            throw new Exception("No Rounds found to get Group standing");
        if (matchType.Players.Count == 0)
            throw new Exception("No players found in the matchtype to get the standing");

        Dictionary<int, PlayerStanding>? playerStandings = matchType
            .Players.Select(p => new PlayerStanding { Player = p })
            .ToDictionary(ps => ps.Player.Id);

        foreach (var round in matchType.Rounds)
        {
            foreach (var match in round.Matches)
            {
                if (match.ScoreJson == null)
                    continue;

                var score = (EightBallPoolScore)DeserializeScore(match.ScoreJson);
                var player1Standing = playerStandings[match.FirstPlayer.Id];
                var player2Standing = playerStandings[match.SecondPlayer.Id];

                // Update match points
                if (score.Player1Racks > score.Player2Racks)
                {
                    player1Standing.Wins++;
                    player2Standing.Losses++;

                    // Match points
                    player1Standing.MatchPoints += 2;
                    player2Standing.MatchPoints += 1;
                }
                else
                {
                    player2Standing.Wins++;
                    player1Standing.Losses++;

                    // Match points
                    player2Standing.MatchPoints += 2;
                    player1Standing.MatchPoints += 1;
                }

                // Update racks score difference
                player1Standing.GamesWon += score.Player1Racks;
                player1Standing.GamesLost += score.Player2Racks;
                player2Standing.GamesWon += score.Player2Racks;
                player2Standing.GamesLost += score.Player1Racks;
            }
        }

        // Convert dictionary to list and sort standings
        var standings = playerStandings.Values.ToList();
        standings.Sort(
            (x, y) =>
            {
                int result = y.MatchPoints.CompareTo(x.MatchPoints);
                if (result == 0)
                    result = y.Wins.CompareTo(x.Wins);
                if (result == 0)
                    result = y.GameDifference.CompareTo(x.GameDifference);
                return result;
            }
        );

        // Check for ties and assign ranks
        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Ranking = i + 1;
            if (
                i > 0
                && standings[i].MatchPoints == standings[i - 1].MatchPoints
                && standings[i].Wins == standings[i - 1].Wins
                && standings[i].GameDifference == standings[i - 1].GameDifference
            )
            {
                standings[i].Ranking = standings[i - 1].Ranking;
                Console.WriteLine(
                    $"A tie Exists between Player {standings[i].Player.Name} and {standings[i - 1].Player.Name}"
                );
            }
        }

        return completeStanding ? standings : standings.Take(tournament.WinnerPerGroup).ToList();
    }
}
