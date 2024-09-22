using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateRoundRobinMatches : ICreateMatch
{
    private readonly string _roundPrefix = "Round";
    private readonly string _byePlayerName = "Bye";
    private readonly string _byePlayerEmail = "bye@gmail.com";
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType)
    {
        //check if match type is group
        if (matchType is not Group group)
        {
            throw new BadRequestException($"Match type must be Group for round robin matches {nameof(matchType)}");
        }
        //get the players of matchtype
        var players = group.Players;

        if (players.Count() % 2 != 0)
        {
            //add a bye player
            players.Add(
                new Player
                {
                    Name = _byePlayerName,
                    Email = _byePlayerEmail,
                }
            );
        }
        //figure out the round number
        var roundNumber = players.Count() - 1;
        //loop through the round and make matches

        //created matches 
        var createdMatches = new List<Match>();

        for (int r = 0; r < roundNumber; r++)
        {
            var round = GetRound(r + 1, matchType);

            for (int i = 0; i < players.Count() / 2; i++)
            {
                var player1 = players[i];
                var player2 = players[players.Count() - 1 - i];

                //check if any player is bye
                if (player1.Name != _byePlayerName && player2.Name != _byePlayerName)
                {
                    //only creating matches here, not scheduling. scheduling will be done my scheduler
                    var roundMatch = GetMatch(player1, player2, tournament, round);
                    createdMatches.Add(roundMatch);
                }
            }

            //fix the first player and rotate the rest
            var roatedPlayer = RotateLeft(players.Skip(1).ToList()); //skip first player
            players = new List<Player> { players[0] }.Concat(roatedPlayer).ToList();

        }

        return Task.FromResult(createdMatches.AsEnumerable());
    }
    private List<Player> RotateLeft(List<Player> players)
    {
        if (players.Count <= 1)
            return players;

        // Remove the first player and append it to the end
        var first = players[0];
        players.RemoveAt(0);
        players.Add(first);

        return players;
    }

    private Match GetMatch(Player player1, Player player2, Tournament tournament, Round round)
    {
        var match = new Match
        {
            FirstPlayer = player1,
            SecondPlayer = player2,
            Tournament = tournament,
            Round = round,
        };
        return match;
    }

    private Round GetRound(int roundNumber, MatchType matchType)
    {
        return new Round
        {
            RoundNumber = roundNumber,
            RoundName = _roundPrefix + " - " + roundNumber,
            MatchType = matchType
        };
    }
}
