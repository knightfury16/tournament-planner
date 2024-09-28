using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.DataSeeder;


public static class Factory
{
    public static List<Tournament> CreateTournaments(int numberOfTournament, string? name = "Test")
    {
        List<Tournament> tounaments = new List<Tournament>();

        for (int i = 0; i < numberOfTournament; i++)
        {
            var tour = new TournamentBuilder()
                .WithName($"{name} Tournament " + (i + 1).ToString())
                .Build();
            tounaments.Add(tour);

        }

        return tounaments;
    }

    public static List<Player> CreatePlayers(int numOfPlayers, string? name = "Test")
    {
        List<Player> players = new List<Player>();

        for (int i = 0; i < numOfPlayers; i++)
        {
            var player = new PlayerBuilder()

                .WithName($"{name} Player " + (i + 1).ToString())
                .Build();
            players.Add(player);
        }
        return players;
    }

    public static List<Admin> CreateAdmin(int numOfAdmin, string? name = "Test")
    {
        List<Admin> admins = new List<Admin>();
        for (int i = 0; i <= numOfAdmin; i++)
        {
            var admin = new AdminBuilder().WithName($"{name} Admin " + (i + 1).ToString())
                .Build();
            admins.Add(admin);
        }
        return admins;
    }
}


public interface ICreateMatchType
{
    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, string? prefix = "Untitled", List<int>? sedderPlayers = null);
}

public class CreateGroupMatchType : ICreateMatchType
{
    private readonly int _maxGroupSize = 5;

    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, string? prefix = "Group", List<int>? seederPlayers = null)
    {
        var numberOfGroup = DetermineNumberOfGroup(tournament.Participants.Count);
        List<MatchType> groups = GenerateGroups(tournament, numberOfGroup, prefix);
        var distributedPlayers = DetermineDistributedPlayers(tournament.Participants, seederPlayers);
        DistributePlayersAmongGroups(distributedPlayers, ref groups);
        return Task.FromResult((IEnumerable<MatchType>?)groups);
    }

    private void DistributePlayersAmongGroups(List<Player> distributedPlayers, ref List<MatchType> groups)
    {
        var distributedPlayersQueue = new Queue<Player>(distributedPlayers);

        for (int i = 0; i < _maxGroupSize; i++)
        {
            foreach (var group in groups)
            {
                if (distributedPlayersQueue.Count > 0)
                {
                    var player = distributedPlayersQueue.Dequeue();
                    group.Players.Add(player);
                }
                else
                {
                    break;
                }

            }
        }

    }

    private List<Player> DetermineDistributedPlayers(List<Player> participants, List<int>? seederPlayerIds)
    {
        Random random = new Random();
        if (seederPlayerIds != null)
        {
            //custom sorting
            participants.Sort((leftPlayer, rightPlayer) =>
           {
               //First level of sorting: Seeder Player
               bool isLeftPlayerSeeder = seederPlayerIds.Contains(leftPlayer.Id);
               bool isRightPlayerSeeder = seederPlayerIds.Contains(rightPlayer.Id);

               if (isLeftPlayerSeeder && !isRightPlayerSeeder) return -1;
               if (!isLeftPlayerSeeder && isRightPlayerSeeder) return 1;

               //Second level of sorting: Win Ratio
               int winRatioComparison = leftPlayer.WinRatio.CompareTo(rightPlayer.WinRatio);
               if (winRatioComparison != 0) return winRatioComparison * -1; //coz i need it in descending order

               //Third level of sorting: Random
               return random.Next().CompareTo(random.Next());

           });

        }
        else
        {

            participants.Sort((leftPlayer, rightPlayer) =>
            {
                //First level of sorting: Win Ratio
                int winRatioComparison = leftPlayer.WinRatio.CompareTo(rightPlayer.WinRatio);
                if (winRatioComparison != 0) return winRatioComparison;

                //Second level of sorting: Random
                return random.Next().CompareTo(random.Next());
            });
        }
        return participants;
    }

    private List<MatchType> GenerateGroups(Tournament tournament, int numberOfGroup, string? prefix)
    {
        var initialChar = 'A';
        List<MatchType> matchTypes = new();

        for (int i = 0; i < numberOfGroup; i++)
        {
            var matchTypeName = $"{prefix}-" + initialChar;
            var matchType = new Group
            {
                Name = matchTypeName,
            };
            matchType.Draw = GetDraw(tournament, matchType);
            matchTypes.Add(matchType);
            initialChar = (char)(initialChar + 1);
        }
        return matchTypes;
    }

    private int DetermineNumberOfGroup(int totalParticipant)
    {
        return (int)Math.Ceiling((double)totalParticipant / _maxGroupSize);
    }

    private Draw GetDraw(Tournament tournament, MatchType matchType)
    {
        return new Draw
        {
            Tournament = tournament,
            MatchType = matchType
        };
    }
}
