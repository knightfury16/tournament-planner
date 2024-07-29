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