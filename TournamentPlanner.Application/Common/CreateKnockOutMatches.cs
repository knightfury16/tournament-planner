using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public class CreateKnockOutMatches : ICreateMatch
{
    private readonly IRepository<MatchType> _matchTypeRepository;

    public CreateKnockOutMatches(IRepository<MatchType> matchTypeRepository)
    {
        _matchTypeRepository = matchTypeRepository;
    }

    public async Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType)
    {
        //is it the first time? does the matchtype contain any round?
        if (matchType == null) throw new ArgumentNullException(nameof(matchType));

        if (matchType.Rounds == null)
        {
            await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.Rounds);
        }

        //if it is knockout tournament
        if (matchType.Rounds?.Count == 0 && tournament.TournamentType == Domain.Enum.TournamentType.Knockout)
        {
            //go make some random pair up
            //match up the player with power of 2
            return await KnockoutTournamentFirstRound(tournament, matchType);

        }
        else if (matchType.Rounds?.Count == 0)
        {
            //if it is group type and im here, that means group section is done. get the group matches standing players and make a match up among them
            // match up the player with power of 2
            return KnockoutAfterGroupFirstRound(tournament, matchType);
        }
        //previous round exists
        //fetch the previous matches winner. 
        //check if the  order remain the same
        throw new NotImplementedException();
    }

    private IEnumerable<Match> KnockoutAfterGroupFirstRound(Tournament tournament, MatchType matchType)
    {
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<Match>> KnockoutTournamentFirstRound(Tournament tournament, MatchType matchType)
    {
        if (matchType.Players.Count == 0) await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.Players);
        int numberOfBye = GetNumberOfBye(matchType.Players);
        int matchToBePlayed = matchType.Players.Count - numberOfBye;

        //shuffle the players total random
        List<Match> matches = new List<Match>();
        Round round = GetRound(1, matchType);

        List<Player> shuffledPlayers = ShuffledPlayers(matchType.Players);
        for (int i = 0; i < matchToBePlayed; i += 2)
        {
            matches.Add(GetMatch(shuffledPlayers[i], shuffledPlayers[i + 1], round, tournament));
        }

        //add the logic in front end to show bye where a player match is not set
        return matches;
    }

    private Match GetMatch(Player player1, Player player2, Round round, Tournament tournament)
    {
        return new Match
        {
            FirstPlayer = player1,
            SecondPlayer = player2,
            Round = round,
            Tournament = tournament
        };
    }

    private Round GetRound(int roundNumber, MatchType matchType)
    {
        var roundName = GetRoundName(matchType.Players.Count);
        return new Round
        {
            RoundNumber = roundNumber,
            RoundName = roundName,
            MatchType = matchType,
        };
    }

    private List<Player> ShuffledPlayers(List<Player> players)
    {
        var random = new Random();
        return players.OrderBy(p => random.Next()).ToList();
    }

    private int GetNumberOfBye(List<Player> players)
    {
        return HighestPowerof2(players.Count) - players.Count;
    }

    private int HighestPowerof2(int N)
    {
        // if N is a power of two simply return it
        if ((N & (N - 1)) == 0)
            return N;
        // else set only the most significant bit
        return 1 << (Convert.ToString(N, 2).Length);
    }

    private string GetRoundName(int playerNumber)
    {
        return playerNumber switch
        {
            8 => "Quater Final",
            4 => "Semi Final",
            2 => "Final",
            _ => "KnockOut"
        };
    }
}
