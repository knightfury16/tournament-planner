using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public interface IKnockout
{

    Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType);
    Task<IEnumerable<Match>> CreateFirstRoundMatches(Tournament tournament, MatchType matchType);
    Task<IEnumerable<Match>> CreateFirstRoundMatchesAfterGroup(Tournament tournament, MatchType matchType, Dictionary<string, List<PlayerStanding>> groupOfPlayerStanding);
}

public class CreateKnockOutMatches : IKnockout
{
    private readonly IRepository<MatchType> _matchTypeRepository;
    private Player? _byePlayer;

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
    public async Task<IEnumerable<Match>> CreateFirstRoundMatches(Tournament tournament, MatchType matchType)
    {
        return await KnockoutTournamentFirstRound(tournament, matchType);
    }

    public async Task<IEnumerable<Match>> CreateFirstRoundMatchesAfterGroup(Tournament tournament, MatchType matchType, Dictionary<string, List<PlayerStanding>> groupOfPlayerStanding)
    {
        if (groupOfPlayerStanding == null || groupOfPlayerStanding.Count == 0) throw new Exception("Group of player standing can not be null or zero");

        var playerPerGroup = groupOfPlayerStanding.First().Value.Count;
        //need to handle odd player case

        //is the gorupOfPlayerStanding even?
        if (groupOfPlayerStanding.Count % 2 != 0)
        {
            List<PlayerStanding> byePlayersStanding = new List<PlayerStanding>();
            for (int i = 0; i < playerPerGroup; i++)
            {

                byePlayersStanding.Add(new PlayerStanding
                {
                    Player = GetByePlayer()
                });
            }

            groupOfPlayerStanding.Add("BYE_GROUP", byePlayersStanding);
        }
        //TODO
        //!! AM writing the code logic with 2 player per group in mind, need to make it dynamic
        // var numberOfByePlayer = GetNumberOfBye(groupOfPlayerStanding.First().Value.Count);
        //is the playerStanding per group power of tw0?

        //shuffle up th groupOfPlayeStanding
        var random = new Random();
        var groupCount = groupOfPlayerStanding.Count;
        int halfGroupCount = groupCount / 2;

        Round round = GetRound(1, matchType);
        List<Match> matches = new List<Match>();

        groupOfPlayerStanding = groupOfPlayerStanding.OrderBy(gr => random.Next()).ToDictionary();

        //per pair of group match from A2-B1, A1-B2... An-B(n-1)
        for (int i = 0; i < halfGroupCount; i++)
        {
            var formerGroup = groupOfPlayerStanding.ElementAt(i).Value;
            var laterGroup = groupOfPlayerStanding.ElementAt((groupCount - 1) - i).Value; //-1 for the index
            for (int numberOfPlayer = 0; numberOfPlayer < playerPerGroup; numberOfPlayer++)
            {
                //first group first player
                var firstPlayer = formerGroup[numberOfPlayer].Player;
                var secondPlayer = laterGroup[(playerPerGroup - 1) - numberOfPlayer].Player;
                matches.Add(GetMatch(firstPlayer, secondPlayer, round, tournament));
            }

        }
        //create matches and return
        return await Task.FromResult(matches);
    }

    private IEnumerable<Match> KnockoutAfterGroupFirstRound(Tournament tournament, MatchType matchType)
    {
        //i nedd to get all the player from all the group standing
        //every draw contain a group
        //get all the draws j
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<Match>> KnockoutTournamentFirstRound(Tournament tournament, MatchType matchType)
    {
        if (matchType.Players.Count == 0) await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.Players);
        int numberOfBye = GetNumberOfBye(matchType.Players.Count);
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

    private int GetNumberOfBye(int count)
    {
        return HighestPowerof2(count) - count;
    }

    private int HighestPowerof2(int N)
    {
        // if N is a power of two simply return it
        if ((N & (N - 1)) == 0)
            return N;
        // else set only the most significant bit
        return 1 << (Convert.ToString(N, 2).Length);
    }

    private Player GetByePlayer()
    {
        if (_byePlayer == null)
        {
            _byePlayer = new Player
            {
                Email = "Bye@gmail.com",
                Name = "Bye player",
                Age = 18,
            };
        }
        return _byePlayer;
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
