using TournamentPlanner.DataModeling;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.DataSeeder;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Data;

public static class Data
{
    public static async Task SeedData(TournamentPlannerDataContext context, int playerCount = 16)
    {
        // Create admins
        var admins = Factory.CreateAdmin(3);
        context.Admins.AddRange(admins);

        // Create Players
        var players = Factory.CreatePlayers(playerCount);
        CreateSomeRandomWin(playerCount, ref players);
        context.Players.AddRange(players);

        // Create tournament
        var tableTennisGameType = await context.GameTypes.FirstOrDefaultAsync(gt => gt.Name == GameTypeSupported.TableTennis);
        var tournament = new TournamentBuilder()
            .WithName("Test Group")
            .WithAdmin(admins[0])
            .WithStatus(TournamentStatus.RegistrationClosed)
            .WithGameType(tableTennisGameType!)
            .WithTournamentType(TournamentType.GroupStage)

            .WithStartDate(DateTime.UtcNow.AddDays(30))
            .Build();

        context.Tournaments.Add(tournament);

        //Add players to tournament
        tournament.Participants.AddRange(players);


        // Save changes to generate IDs
        await context.SaveChangesAsync();

        //Will make draws here


        //// Create groups
        //var groupA = new Group { Name = "Group A" };
        //var groupB = new Group { Name = "Group B" };

        //// Assign players to groups
        //groupA.Players.AddRange(players.Take(4));
        //groupB.Players.AddRange(players.Skip(4).Take(2));
        //context.Groups.AddRange(groupA, groupB);  

        //// Create matches
        //var matches = new List<Match>();
        //foreach (var group in new[] { groupA, groupB })
        //{
        //    var round = GetRound(group);
        //    for (int i = 0; i < group.Players.Count; i++)
        //    {
        //        for (int j = i + 1; j < group.Players.Count; j++)
        //        {
        //            var match = new MatchBuilder()
        //                .WithPlayers(group.Players[i], group.Players[j])
        //                .WithTournament(tournament)
        //                .WithRound(round)
        //                .Build();
        //            matches.Add(match);
        //        }
        //    }
        //}
        //context.Matches.AddRange(matches);

        //// Simulate some matches
        //var random = new Random();
        //foreach (var match in matches.Take(matches.Count / 2))
        //{
        //    match.GamePlayed = DateTime.UtcNow;
        //    match.Winner = random.Next(2) == 0 ? match.FirstPlayer : match.SecondPlayer;
        //    match.WinnerId = match.Winner.Id;

        //    // Updated score simulation for table tennis
        //    var score = new TableTennisScore
        //    {
        //        SetScores = new List<SetScore>()
        //    };

        //    for (int i = 0; i < 5; i++) // Simulate up to 5 sets
        //    {
        //        var player1Points = random.Next(11, 15);
        //        var player2Points = random.Next(9, 11);
        //        if (match.Winner == match.SecondPlayer)
        //        {
        //            (player1Points, player2Points) = (player2Points, player1Points);
        //        }
        //        score.SetScores.Add(new SetScore { Player1Points = player1Points , Player2Points = player2Points});

        //        if (score.SetScores.Count(s => s.Player1Points > s.Player2Points) == 3 ||
        //            score.SetScores.Count(s => s.Player2Points > s.Player1Points) == 3)
        //        {
        //            break; // One player has won 3 sets
        //        }
        //    }

        //    score.Player1Sets = score.SetScores.Count(s => s.Player1Points > s.Player2Points);
        //    score.Player2Sets = score.SetScores.Count(s => s.Player2Points > s.Player1Points);

        //    match.ScoreJson = JsonSerializer.Serialize(score);
        //}

        // Save all changes
        //await context.SaveChangesAsync();

        Console.WriteLine("Seed data created successfully!");
    }

    private static void CreateSomeRandomWin(int numOfPlayer, ref List<Player> players)
    {
        var rand = new Random();
        for (int i = 0; i < numOfPlayer; i++)
        {
            var randIndx = rand.Next(players.Count() - 1);
            players[randIndx].GamePlayed = rand.Next(10, 12);
            players[randIndx].GameWon = rand.Next(1, 9);
        }

    }

    private static Round GetRound(Group group)
    {
        return new RoundBuilder()
            .WithMatchType(group)
            .WithRoundNumber(1)
            .WithRoundName(group.Name)
            .Build();
    }

    public static void RemoveAllDataBeforeSeedAndSave(TournamentPlannerDataContext context)
    {
        // Clear existing data
        context.Tournaments.RemoveRange(context.Tournaments);
        context.Players.RemoveRange(context.Players);
        context.Admins.RemoveRange(context.Admins);
        context.Rounds.RemoveRange(context.Rounds);
        context.Matches.RemoveRange(context.Matches);
        context.MatchTypes.RemoveRange(context.MatchTypes);
        //context.GameTypes.RemoveRange(context.GameTypes);//its preseeded on migration

        context.SaveChanges();

    }

}
