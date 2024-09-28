using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateGroupMatchType : ICreateMatchType
{
    private int _maxGroupSize = 5;


    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, List<Player> players, string? prefix, List<int>? seederPlayerIds)
    {
        prefix ??= "Group";
        var numberOfGroup = DetermineNumberOfGroup(players.Count);
        List<MatchType> groups = GenerateGroups(tournament, numberOfGroup, prefix);
        var sortedPlayers = GetSortedPlayers(players, seederPlayerIds);
        DistributePlayersAmongGroups(sortedPlayers, ref groups);
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

    private List<Player> GetSortedPlayers(List<Player> participants, List<int>? seederPlayerIds = null)
    {
        //getting a list of player by seeder, power, then by randomly to distribute similar level player evenly among groups
        Random random = new Random();

        //*custom sorting for learning purpose
        if (seederPlayerIds != null)
        {
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
                if (winRatioComparison != 0) return winRatioComparison * -1; //coz i need it in descending order

                //Second level of sorting: Random
                return random.Next().CompareTo(random.Next());
            });
        }
        return participants;
        //*Previous implementation
        // return participants.OrderBy(p => p.WinRatio).ThenBy(p => random.Next()).Reverse().ToList();
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
            matchTypes.Add(matchType);
            initialChar = (char)(initialChar + 1);
        }
        return matchTypes;
    }
    private int DetermineNumberOfGroup(int totalParticipant)
    {
        return (int)Math.Ceiling((double)totalParticipant / _maxGroupSize);
    }
}
