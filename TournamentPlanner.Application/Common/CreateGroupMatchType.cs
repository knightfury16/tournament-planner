using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateGroupMatchType : ICreateMatchType
{
    private int _maxGroupSize = 5;


    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, string? prefix)
    {
        prefix ??= "Group";
        var numberOfGroup = DetermineNumberOfGroup(tournament.Participants.Count);
        List<MatchType> groups = GenerateGroups(tournament, numberOfGroup, prefix);
        var distributedPlayers = DetermineDistributedPlayers(tournament.Participants);
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

    private List<Player> DetermineDistributedPlayers(List<Player> participants)
    {
        Random random = new Random();
        //getting a list of player by power, then by randomly to distribute similar level player evenly among groups
        return participants.OrderBy(p => p.WinRatio).ThenBy(p => random.Next()).Reverse().ToList();
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
            //TODO: Handle this place gracefully
            matchType.Draw = GetDraw(tournament, matchType);
            matchTypes.Add(matchType);
            initialChar = (char)(initialChar + 1);
        }
        return matchTypes;
    }
    private Draw GetDraw(Tournament tournament, MatchType matchType)
    {
        return new Draw
        {
            Tournament = tournament,
            MatchType = matchType
        };
    }
    private int DetermineNumberOfGroup(int totalParticipant)
    {
        return (int)Math.Ceiling((double)totalParticipant / _maxGroupSize);
    }
}
