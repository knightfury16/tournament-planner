using System.Text.Json;
using TournamentPlanner.Application.GameTypeScore;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Interface;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

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

    public override List<PlayerStanding> GetGroupStanding(Tournament tournament, MatchType matchType, bool completeStanding = false)
    {
        if (tournament == null || matchType == null) throw new ArgumentNullException(nameof(GetGroupStanding));

        var playerStandings = matchType.Players.Select(p => new PlayerStanding { Player = p }).ToDictionary(ps => ps.Player.Id);

        foreach (var round in matchType.Rounds)
        {
            foreach (var match in round.Matches)
            {
                if (match.ScoreJson == null) continue;

                var score = (TableTennisScore)DeserializeScore(match.ScoreJson);
                var player1Standing = playerStandings[match.FirstPlayer.Id];
                var player2Standing = playerStandings[match.SecondPlayer.Id];

                // Update game points
                player1Standing.GamePoints += score.Player1Sets;
                player2Standing.GamePoints += score.Player2Sets;

                // Update wins and losses
                if (score.Player1Sets > score.Player2Sets)
                {
                    player1Standing.Wins++;
                    player2Standing.Losses++;

                    //match points
                    player1Standing.MatchPoints += 2;
                    player2Standing.MatchPoints += 1;

                    //game difference
                    player1Standing.GamesWon += score.Player1Sets;
                    player2Standing.GamesLost += score.Player2Sets;

                }
                else
                {
                    player2Standing.Wins++;
                    player1Standing.Losses++;

                    //match points
                    player2Standing.MatchPoints += 2;
                    player1Standing.MatchPoints += 1;

                    //game difference
                    player2Standing.GamesWon += score.Player2Sets;
                    player1Standing.GamesLost += score.Player1Sets;
                }


                // Update points difference
                foreach (var setScore in score.SetScores)
                {
                    //player 1 points won
                    player1Standing.PointsWon += setScore.Player1Points;
                    //player 1 points lost  
                    player1Standing.PointsLost += setScore.Player2Points;
                    //player 2 points won
                    player2Standing.PointsWon += setScore.Player2Points;
                    //player 2 points lost
                    player2Standing.PointsLost += setScore.Player1Points;
                }
            }
        }

        // Convert dictionary to list and sort standings
        var standings = playerStandings.Values.ToList();
        standings.Sort((x, y) =>
        {
            int result = y.GamePoints.CompareTo(x.GamePoints);
            if (result == 0) result = y.Wins.CompareTo(x.Wins);
            if (result == 0) result = y.GameDifference.CompareTo(x.GameDifference);
            if (result == 0) result = y.PointsDifference.CompareTo(x.PointsDifference);
            return result;
        });

        // Check for ties and assign ranks
        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Ranking = i + 1;
            if (i > 0 && standings[i].GamePoints == standings[i - 1].GamePoints &&
                standings[i].Wins == standings[i - 1].Wins &&
                standings[i].GameDifference == standings[i - 1].GameDifference &&
                standings[i].PointsDifference == standings[i - 1].PointsDifference)
            {
                standings[i].Ranking = standings[i - 1].Ranking;
            }
        }

        return completeStanding ? standings : standings.Take(tournament.WinnerPerGroup).ToList();
    }

    public override IScore CreateInitialScore()
    {
        return new TableTennisScore();
    }
}

