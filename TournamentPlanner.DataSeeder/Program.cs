using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Data;
using TournamentPlanner.DataModeling;
using TournamentPlanner.DataSeeder;
using TournamentPlanner.Domain.Entities;

const bool ON_TEST = true;
//can only seed the data from here, can not create the migration as i need the EFDesigner to 
//to the actual migration
var factory = new SeederFactory(ON_TEST);
// var factory = new TournamentPlannerDataContextFactory();

var dataContext = factory.CreateDbContext(null);

bool shouldRemove = false;
bool shouldSeed = false;
bool shouldAdd = false;
int playerCount = 16;
int tournamentId = 0;

foreach (var arg in args)
{
    switch (arg.ToLower())
    {
        case "seed":
            shouldSeed = true;
            break;
        case "remove":
            shouldRemove = true;
            break;
        case "add":
            shouldAdd = true;
            break;
    }
}

// Add this block after the foreach loop
if (args.Length > 0)
{
    foreach (var arg in args)
    {
        if (arg.StartsWith("playercount="))
        {
            string playerCountArg = arg.Split('=')[1];
            if (int.TryParse(playerCountArg, out int parsedPlayerCount))
            {
                playerCount = parsedPlayerCount;
                Console.WriteLine($"Player count set to: {playerCount}");
            }
            else
            {
                Console.WriteLine($"Invalid player count: {playerCountArg}. Using default: {playerCount}");
            }
        }
        else if (arg.StartsWith("tournamentId="))
        {
            string tournamentIdArg = arg.Split('=')[1];
            if (int.TryParse(tournamentIdArg, out int parsedTournamentId))
            {
                tournamentId = parsedTournamentId;
                Console.WriteLine($"Tournament ID set to: {tournamentId}");
            }
            else
            {
                throw new Exception($"Invalid tournament ID: {tournamentIdArg}");
            }
        }
    }
}

if (shouldRemove)
{
    await RemoveAllData(dataContext);
    Console.WriteLine("All data removed.");
}

if (shouldSeed)
{
    await SeedData(dataContext);
    Console.WriteLine("Data seeded.");
}
if (shouldAdd)
{
    await AddPlayerToTournament(dataContext);
    System.Console.WriteLine("Added player to tournament");
}


if (!shouldRemove && !shouldSeed && !shouldAdd)
{
    Console.WriteLine("Please provide arguments: 'remove' to remove all data and/or 'seed' to seed data.");
    Console.WriteLine("Example: dotnet run remove seed");
}

async Task SeedData(TournamentPlannerDataContext context)
{
    using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        await Data.SeedData(context, playerCount);
        await transaction.CommitAsync();
        Console.WriteLine("Simulated success!!");

    }
    catch (Exception e)
    {
        Console.Error.WriteLine($"someting went wrong: {e.Message}");
    }
}

async Task RemoveAllData(TournamentPlannerDataContext dataContext)
{
    // Clear existing data
    await Task.Run(() => Data.RemoveAllDataBeforeSeedAndSave(dataContext));
}


async Task AddPlayerToTournament(TournamentPlannerDataContext dataContext)
{
    System.Console.WriteLine($"Adding {playerCount} to tournament with Id {tournamentId}");
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
    var rand = new Random();
    var playerToAdd = Factory.CreatePlayers(playerCount, $"TestFromAdd{rand.Next()}");
    dataContext.Players.AddRange(playerToAdd);
    tournament?.Participants.AddRange(playerToAdd);
    await dataContext.SaveChangesAsync();
    System.Console.WriteLine("Added player to tournament sccessfully!!");
}