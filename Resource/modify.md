```c#
using System;
using System.Collections.Generic;

namespace TournamentPlanner.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public abstract class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        // Added to support email notifications
        public List<GameType> InterestedGameTypes { get; set; } = new List<GameType>();
    }

    public class Admin : User
    {
        public required string PhoneNumber { get; set; }
    }

    public class Player : User
    {
        public int Age { get; set; }
        public int Weight { get; set; }
        public List<int> TournamentParticipatedId { get; set; } = new List<int>();
        public int GamePlayed { get; set; }
        public int GameWon { get; set; }
        public double WinRatio => GamePlayed > 0 ? (double)GameWon / GamePlayed : 0;
    }

    // Changed from abstract class to regular class to support the InterestedGameTypes property in User
    public class GameType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public abstract class GameFormat<TScore> where TScore : IScore
    {
        public required GameType GameType { get; set; }
        public abstract TScore CreateInitialScore();
        public abstract bool IsValidScore(TScore score);
        public abstract Player DetermineWinner(Player player1, Player player2, TScore score);
    }

    public class Tournament : BaseEntity
    {
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RegistrationLastDate { get; set; }
        public int MaxParticipant { get; set; }
        public string? Venue { get; set; }
        public decimal RegistrationFee { get; set; }
        public int MinimumAgeOfRegistration { get; set; }
        public int WinnerPerGroup { get; set; }
        public int KnockOutStartNumber { get; set; }
        public ResolutionStrategy ParticipantResolutionStrategy { get; set; }
        public required TournamentType TournamentType { get; set; }
        public required GameType GameType { get; set; }
        public List<Player> Participants { get; set; } = new();
        public List<MatchType> Groups { get; set; } = new();
        public List<MatchType> KnockOuts { get; set; } = new();
        public List<Match> Matches { get; set; } = new List<Match>();
        // Added to support tournament status
        public TournamentStatus Status { get; set; }
        // Added to support search functionality
        public bool IsSearchable => Status != TournamentStatus.Draft;
    }

    public abstract class MatchType : BaseEntity
    {
        public required string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Match> Matches { get; set; } = new List<Match>();
    }

    public class Group : MatchType
    {
    }

    public class KnockOut : MatchType
    {
        public int Round { get; set; }
    }

    public class Match : BaseEntity
    {
        public required Player FirstPlayer { get; set; }
        public required Player SecondPlayer { get; set; }
        public Player? Winner { get; set; }
        public DateTime? GameScheduled { get; set; }
        public DateTime? GamePlayed { get; set; }
        public IScore? Score { get; set; }
        // Added to support match rescheduling
        public bool IsRescheduled { get; set; }
        public string? RescheduleReason { get; set; }
        public Admin? RescheduledBy { get; set; }
    }

    public interface IScore
    {
        bool IsComplete { get; }
    }

    public enum ResolutionStrategy
    {
        StatBased,
        Random,
        KnockoutQualifier
    }

    public enum TournamentType
    {
        GroupStage,
        Knockout
    }

    // Added to support tournament status
    public enum TournamentStatus
    {
        Draft,
        RegistrationOpen,
        RegistrationClosed,
        Ongoing,
        Completed
    }

    public class TableTennisFormat : GameFormat<TableTennisScore>
    {
        public int SetsToWin { get; set; } = 3;
        public int PointsPerSet { get; set; } = 11;

        public override TableTennisScore CreateInitialScore()
        {
            return new TableTennisScore();
        }

        public override bool IsValidScore(TableTennisScore score)
        {
            if (score.Player1Sets + score.Player2Sets > SetsToWin * 2 - 1)
                return false;

            foreach (var setScore in score.SetScores)
            {
                if (setScore.Player1Points < PointsPerSet && setScore.Player2Points < PointsPerSet)
                    return false;
                if (Math.Abs(setScore.Player1Points - setScore.Player2Points) < 2)
                    return false;
            }

            return true;
        }

        public override Player DetermineWinner(Player player1, Player player2, TableTennisScore score)
        {
            return score.Player1Sets > score.Player2Sets ? player1 : player2;
        }
    }

    public class TableTennisScore : IScore
    {
        public int Player1Sets { get; set; }
        public int Player2Sets { get; set; }
        public List<(int Player1Points, int Player2Points)> SetScores { get; set; } = new List<(int, int)>();
        public bool IsComplete => Player1Sets == 3 || Player2Sets == 3;
    }
}
```


## Concerete example
```c#
using System;
using System.Linq;
using System.Collections.Generic;
using TournamentPlanner.Domain.Entities;

class Program
{
    static void Main()
    {
        // 1. Create an admin
        var admin = new Admin
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890"
        };
        Console.WriteLine($"Admin created: {admin.Name}");

        // 2. Create players
        var player1 = new Player
        {
            Name = "Alice Smith",
            Email = "alice.smith@example.com",
            Age = 25
        };
        var player2 = new Player
        {
            Name = "Bob Johnson",
            Email = "bob.johnson@example.com",
            Age = 28
        };
        Console.WriteLine($"Players created: {player1.Name}, {player2.Name}");

        // 3. Create a game type
        var tableTennisGameType = new GameType
        {
            Id = 1,
            Name = "Table Tennis"
        };

        // 4. Create a tournament
        var tournament = new Tournament
        {
            Name = "Summer Table Tennis Championship",
            StartDate = DateTime.Now.AddDays(30),
            EndDate = DateTime.Now.AddDays(32),
            RegistrationLastDate = DateTime.Now.AddDays(25),
            MaxParticipant = 16,
            Venue = "City Sports Center",
            RegistrationFee = 50.00m,
            MinimumAgeOfRegistration = 18,
            WinnerPerGroup = 2,
            KnockOutStartNumber = 8,
            ParticipantResolutionStrategy = ResolutionStrategy.StatBased,
            TournamentType = TournamentType.GroupStage,
            GameType = tableTennisGameType,
            Status = TournamentStatus.RegistrationOpen
        };
        Console.WriteLine($"Tournament created: {tournament.Name}");

        // 5. Register players to the tournament
        tournament.Participants.Add(player1);
        tournament.Participants.Add(player2);
        player1.TournamentParticipatedId.Add(tournament.Id);
        player2.TournamentParticipatedId.Add(tournament.Id);
        Console.WriteLine($"Players registered: {player1.Name}, {player2.Name}");

        // 6. Create groups for the tournament (assuming 4 groups)
        for (int i = 1; i <= 4; i++)
        {
            var group = new Group
            {
                Name = $"Group {i}",
                Players = new List<Player>()
            };
            tournament.Groups.Add(group);
        }
        Console.WriteLine($"Groups created: {tournament.Groups.Count}");

        // 7. Assign players to groups (simplified version)
        tournament.Groups[0].Players.Add(player1);
        tournament.Groups[1].Players.Add(player2);
        Console.WriteLine("Players assigned to groups");

        // 8. Create matches for the group stage
        foreach (var group in tournament.Groups)
        {
            for (int i = 0; i < group.Players.Count; i++)
            {
                for (int j = i + 1; j < group.Players.Count; j++)
                {
                    var match = new Match
                    {
                        FirstPlayer = group.Players[i],
                        SecondPlayer = group.Players[j],
                        GameScheduled = tournament.StartDate.AddHours(i * 2) // Simple scheduling
                    };
                    group.Matches.Add(match);
                    tournament.Matches.Add(match);
                }
            }
        }
        Console.WriteLine($"Matches created: {tournament.Matches.Count}");

        // 9. Start the tournament
        tournament.Status = TournamentStatus.Ongoing;
        Console.WriteLine("Tournament started");

        // 10. Play a match and record the result
        var firstMatch = tournament.Matches.First();
        var tableTennisFormat = new TableTennisFormat { GameType = tableTennisGameType };
        var score = new TableTennisScore
        {
            Player1Sets = 3,
            Player2Sets = 1,
            SetScores = new List<(int, int)> { (11, 5), (11, 9), (9, 11), (11, 7) }
        };
        firstMatch.Score = score;
        firstMatch.Winner = tableTennisFormat.DetermineWinner(firstMatch.FirstPlayer, firstMatch.SecondPlayer, score);
        firstMatch.GamePlayed = DateTime.Now;
        Console.WriteLine($"Match played: {firstMatch.FirstPlayer.Name} vs {firstMatch.SecondPlayer.Name}, Winner: {firstMatch.Winner.Name}");

        // 11. Request a match reschedule
        var matchToReschedule = tournament.Matches.Last();
        matchToReschedule.IsRescheduled = true;
        matchToReschedule.RescheduleReason = "Player illness";
        matchToReschedule.RescheduledBy = admin;
        matchToReschedule.GameScheduled = matchToReschedule.GameScheduled?.AddDays(1);
        Console.WriteLine($"Match rescheduled: {matchToReschedule.FirstPlayer.Name} vs {matchToReschedule.SecondPlayer.Name}");

        // 12. Get all matches of the tournament
        var allMatches = tournament.Matches;
        Console.WriteLine($"Total matches in tournament: {allMatches.Count}");

        // 13. Get matches for a specific player
        var playerMatches = tournament.Matches.Where(m => m.FirstPlayer == player1 || m.SecondPlayer == player1).ToList();
        Console.WriteLine($"Matches for {player1.Name}: {playerMatches.Count}");

        // 14. End the tournament
        tournament.Status = TournamentStatus.Completed;
        Console.WriteLine("Tournament completed");
    }
}
```