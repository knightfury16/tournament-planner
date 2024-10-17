using TournamentPlanner.Data;
using TournamentPlanner.DataModeling;
using TournamentPlanner.DataSeeder;

const bool ON_TEST = true;
//can only seed the data from here, can not create the migration as i need the EFDesigner to 
//to the actual migration
var factory = new SeederFactory(ON_TEST);
// var factory = new TournamentPlannerDataContextFactory();

var dataContext = factory.CreateDbContext(null);
await RemoveAllData(dataContext);
await SeedData(dataContext);

async Task SeedData(TournamentPlannerDataContext context)
{
  using var transaction = await context.Database.BeginTransactionAsync();

  try
  {
    await Data.SeedData(context);
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
