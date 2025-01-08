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
        List<Admin> admins = new List<Admin>();
        //search admin in the db with ID-1,  will make all the seed data with id 1
        var admin = await context.Admins.FindAsync(1);
        if (admin != null)
        {
            admins.Add(admin);
        }
        else
        {
            // Create admins
            admins.AddRange(Factory.CreateAdmin(3));
            context.Admins.AddRange(admins);
        }

        // Create Players
        var players = Factory.CreatePlayers(playerCount);
        CreateSomeRandomWin(playerCount, ref players);
        context.Players.AddRange(players);

        // Create tournament
        var tableTennisGameType = await context.GameTypes.FirstOrDefaultAsync(gt => gt.Name == GameTypeSupported.TableTennis);
        var tournament = new TournamentBuilder()
            .WithName($"{SeedDataDefault.TestTournament} Inter Group championship")
            .WithAdmin(admins[0])
            .WithStatus(TournamentStatus.RegistrationClosed)
            .WithGameType(tableTennisGameType!)
            .WithTournamentKnockoutStartNumber(8)
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

    public async static Task AddPlayerToTournament(TournamentPlannerDataContext dataContext, int playerCount, int tournamentId)
    {

        System.Console.WriteLine($"Adding {playerCount} players to tournament with Id {tournamentId}");
        var tournament = await dataContext.Tournaments.Include(t => t.Participants).Where(t => t.Id == tournamentId).FirstOrDefaultAsync();
        if (tournament == null)
        {
            System.Console.WriteLine("Tournament not found to add player");
            return;
        }
        if (tournament?.MaxParticipant < playerCount || tournament?.Participants.Count + playerCount > tournament?.MaxParticipant)
        {
            System.Console.WriteLine("Player count excede the max participants count.");
            return;
        }
        var playerToAdd = await GetPlayerToAdd(dataContext, playerCount);
        ArgumentNullException.ThrowIfNull(playerToAdd);
        dataContext.Players.AddRange(playerToAdd);
        tournament?.Participants.AddRange(playerToAdd);
        await dataContext.SaveChangesAsync();
        System.Console.WriteLine("Added player to tournament sccessfully!!");
    }

    private static async Task<List<Player>> GetPlayerToAdd(TournamentPlannerDataContext dataContext, int playerCount)
    {
        var moqPlayers = Factory.GetMoqPlayers();
        var players = new List<Player>();
        var index = 0;
        while (players.Count < playerCount)
        {
            var player = moqPlayers[index];
            var result = await dataContext.Players.SingleOrDefaultAsync(p => p.Email == player.Email);
            if(result != null)continue;
            players.Add(player);
            index++;
        }
        return players;
    }

    public static void RemoveAllDataBeforeSeedAndSave(TournamentPlannerDataContext context)
    {
        // Clear existing data
        RemoveTournamentsOnlyCreatedBySeeder(context);
        RemovePlayersOnlyCreatedBySeeder(context);
        RemoveAdminsOnlyCreatedBySeeder(context);

        //TODO: convert this remove to remove seed data only
        context.Rounds.RemoveRange(context.Rounds);
        context.Matches.RemoveRange(context.Matches);
        context.MatchTypes.RemoveRange(context.MatchTypes);
        //context.GameTypes.RemoveRange(context.GameTypes);//its preseeded on migration

        context.SaveChanges();

    }

    private static void RemoveTournamentsOnlyCreatedBySeeder(TournamentPlannerDataContext context)
    {
        var seederTournaments = context.Tournaments.Where(t => t.Name.Contains(SeedDataDefault.TestTournament));
        context.RemoveRange(seederTournaments);
    }

    private static void RemoveAdminsOnlyCreatedBySeeder(TournamentPlannerDataContext context)
    {
        var seederAdmins = context.Admins.Where(a => a.Email.Contains(SeedDataDefault.TestAdmin));
        context.Admins.RemoveRange(seederAdmins);
    }

    private static void RemovePlayersOnlyCreatedBySeeder(TournamentPlannerDataContext context)
    {
        var players = context.Players.Where(p => p.Email.Contains(SeedDataDefault.TestPlayer));
        context.Players.RemoveRange(players);
    }
}
