# Desing Overhaul(PENDING ON DECISION)

Need Complete design overhaul.

Was doing the project for single player and fixed 32 player knockout match.

Now I want to do it in generic with lot of control.

# User Entity

There will two type of user entity:

1. Player:
    - This user will be the entity who can participate in tournament.
    - Need lot of information about this entity to do proper analysis or stat calculation. For Example: Age, Weight, TournamentParticipated, GamePlayed, GameWin etc. 

2. Admin:
 - This entity can set a tournament.
 - While setting the tournament admin can set the max number of participant.
 - While setting the tournament, this entity can choose what type of tournament this will be, for example,
 Gorup Stage or Knockout, if GroupStaged then have to select the **number of winner per group** who will proceede to next, and **number** from which knockout will start. If knockout than participant number has to be **power of 2**.
 - If suppose we get less registered player than the max, then depending on the Admin, can choose a resolution strategy. For example, in a knockout match of **16** we get **11** participant. Then obviously we have to start with **8**, but which **8**? Then we can say based on player stat, if stat are all same then randomly take the strongest **5** who will proceed to next, and taking remaining **6** we start a knockout match and **3** winner of those match will proceed to next with the **8** and the usal logic will take place from now. This is one policy that can be applied. As said it depends on the Admin decision. 
 - In group tournament type, group will be created based on the **number of winner per group** who will proceed to next and **knockout start** number. Default value for **number of winner per group** is **2** and knockout start is **8**.
 - In each group, each player will play a match with each one of the player in group at once, **Round Robin**.
 - So this grouping is important, we can not put possible two final candidate, that is stat is very strong, in same group.
 - So based on player/team stat group creation will be evenely distributed, where all group will have an evenly distribution of **some stat value** of player/team.

 - Each Game have their **format of score**. So there can not be a generic score format. We can implement a system where for each specific game we have specific template published.
 - For now I have decided to work on **Tabel Tennis** format. 
 - This format will be set based on the tournamnet game type that will be set by admin during tournament creation.



One pausible solution:

```c#
using System;
using System.Collections.Generic;

namespace TournamentPlanner.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Player : User
    {
        public int Age { get; set; }
        public double Weight { get; set; }
        public int TournamentsParticipated { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public double WinRatio => GamesPlayed > 0 ? (double)GamesWon / GamesPlayed : 0;
    }

    public class Admin : User
    {
        // Additional admin-specific properties can be added here if needed
    }

    public enum TournamentType
    {
        GroupStage,
        Knockout
    }

    public enum ResolutionStrategy
    {
        StatBased,
        Random,
        KnockoutQualifier
        // Add more strategies as needed
    }

    public interface IScore
    {
        bool IsComplete { get; }
    }

    public abstract class GameFormat<TScore> where TScore : IScore
    {
        public string Name { get; set; }
        public abstract TScore CreateInitialScore();
        public abstract bool IsValidScore(TScore score);
        public abstract Player DetermineWinner(Player player1, Player player2, TScore score);
    }

    public class Tournament<TScore> where TScore : IScore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TournamentType Type { get; set; }
        public int MaxParticipants { get; set; }
        public int WinnersPerGroup { get; set; }
        public int KnockoutStartNumber { get; set; }
        public ResolutionStrategy ParticipantResolutionStrategy { get; set; }
        public GameFormat<TScore> GameFormat { get; set; }
        public List<Player> Participants { get; set; } = new List<Player>();
        public List<Group<TScore>> Groups { get; set; } = new List<Group<TScore>>();
        public List<Match<TScore>> Matches { get; set; } = new List<Match<TScore>>();
    }

    public class Group<TScore> where TScore : IScore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Match<TScore>> Matches { get; set; } = new List<Match<TScore>>();
    }

    public class Match<TScore> where TScore : IScore
    {
        public Guid Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player Winner { get; set; }
        public TScore Score { get; set; }
    }

    public class TableTennisScore : IScore
    {
        public int Player1Sets { get; set; }
        public int Player2Sets { get; set; }
        public List<(int Player1Points, int Player2Points)> SetScores { get; set; } = new List<(int, int)>();
        public bool IsComplete => Player1Sets == 3 || Player2Sets == 3;
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

    public class ChessScore : IScore
    {
        public enum ChessResult { Ongoing, Player1Win, Player2Win, Draw }
        public ChessResult Result { get; set; } = ChessResult.Ongoing;
        public List<string> Moves { get; set; } = new List<string>();
        public bool IsComplete => Result != ChessResult.Ongoing;
    }

    public class ChessFormat : GameFormat<ChessScore>
    {
        public override ChessScore CreateInitialScore()
        {
            return new ChessScore();
        }

        public override bool IsValidScore(ChessScore score)
        {
            return true; // In chess, any valid game state is a valid score
        }

        public override Player DetermineWinner(Player player1, Player player2, ChessScore score)
        {
            return score.Result == ChessScore.ChessResult.Player1Win ? player1 :
                   score.Result == ChessScore.ChessResult.Player2Win ? player2 : null; // null for draw or ongoing
        }
    }
}
```
