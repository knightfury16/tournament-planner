using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common;

public interface IKnockout
{

    Task<IEnumerable<Match>> CreateFirstRoundMatches(Tournament tournament, MatchType matchType);
    Task<IEnumerable<Match>> CreateFirstRoundMatchesAfterGroup(Tournament tournament, MatchType matchType, Dictionary<string, List<PlayerStanding>> groupOfPlayerStanding);
    Task<IEnumerable<Match>> CreateSubsequentMatches(Tournament tournament, MatchType matchType);
}

public class CreateKnockOutMatches : IKnockout
{
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IRepository<Round> _roundRepository;
    private readonly IRepository<Player> _playerRepository;
    private Player? _byePlayer;

    public CreateKnockOutMatches(IRepository<MatchType> matchTypeRepository, IRepository<Round> roundRepository, IRepository<Player> playerRepository)
    {
        _matchTypeRepository = matchTypeRepository;
        _roundRepository = roundRepository;
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<Match>> CreateFirstRoundMatches(Tournament tournament, MatchType matchType)
    {
        return await KnockoutTournamentFirstRound(tournament, matchType);
    }

    public async Task<IEnumerable<Match>> CreateFirstRoundMatchesAfterGroup(Tournament tournament, MatchType matchType, Dictionary<string, List<PlayerStanding>> groupOfPlayerStanding)
    {
        if (groupOfPlayerStanding == null || groupOfPlayerStanding.Count == 0) throw new Exception("Group of player standing can not be null or zero");


        //-- Locking Tournament.WinnerPerGroup to 2. And im creating with formula, group = ceil(Tournament.KnckoutStartNumber/Tournament.WinnerPerGroup)
        //-- Tournament.KnockoutStartNumber is restricted to power of 2
        //-- So I will always have even number of group and groupNumber * 2 = will always yeild a power of two
        //-- But I can have ONE groupOfPlayerStanding with one player like -> this case is not possible since im checking for knockout start number to be less than registered player



        //is the gorupOfPlayerStanding even?
        if (groupOfPlayerStanding.Count % 2 != 0)
        {
            //if im here then my gorup making logic is wrong read the above comment
            throw new ValidationException("Group of player standing is Odd");
        }

        if (groupOfPlayerStanding.Values.Any(group => group.Count < 2))
        {
            throw new ValidationException("A group of player standing must contain at least 2 players.");
        }

        //shuffle up th groupOfPlayeStanding
        var random = new Random();
        var groupCount = groupOfPlayerStanding.Count;
        int halfGroupCount = groupCount / 2;
        var playerPerGroup = groupOfPlayerStanding.First().Value.Count; // lets hope first group is not the group with exception that is 1 player

        Round round = GetRound(1, matchType, matchType.Players.Count);
        List<Match> matches = new List<Match>();

        //shuffling group
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


    private async Task<IEnumerable<Match>> KnockoutTournamentFirstRound(Tournament tournament, MatchType matchType)
    {
        if (matchType.Players.Count == 0) await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.Players);
        //load all the seeded players
        if(matchType.SeededPlayers == null || matchType.SeededPlayers.Count == 0)
        {
            await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.SeededPlayers);
        }
        
        int totalSlots = HighestPowerof2Ceil(matchType.Players.Count);
        int numberOfBye = totalSlots - matchType.Players.Count;

        List<Match> matches = new List<Match>();
        Round round = GetRound(1, matchType, totalSlots); // this is the first round
        var allPlayerList = new List<Player>(matchType.Players);

        if(numberOfBye > 0 && matchType.SeededPlayers?.Count > 0)
        {
            //remove the seeded players from the allPlayerList
            foreach(var seededPlayer in matchType.SeededPlayers)
            {
                allPlayerList.Remove(seededPlayer.Player!); //i know players cant be empty here
            }

            var seededPlayerList = matchType.SeededPlayers.Select(sp => sp.Player).ToList();
            int byeMatchCount = 0;
            while(numberOfBye > 0 && byeMatchCount < seededPlayerList.Count )
            {
                var match = GetMatch(await GetByePlayer(), seededPlayerList[byeMatchCount]!, round, tournament);
                matches.Add(match);
                numberOfBye--;
                byeMatchCount++;
            }
        }

        List<Player> shuffledPlayers = ShuffledPlayers(allPlayerList);

        //assign byes to the remaining slots
        for(int i = 0; i < numberOfBye; i++)
        {
            matches.Add(GetMatch(await GetByePlayer(), shuffledPlayers[i], round, tournament));
        }


        //create match for the remaining slots
        for (int i = numberOfBye; i < shuffledPlayers.Count; i += 2)
        {
            if(i + 1 < shuffledPlayers.Count)
            {
                var firstPlayer = shuffledPlayers[i];
                var secondPlayer = shuffledPlayers[i + 1];
                matches.Add(GetMatch(firstPlayer, secondPlayer, round, tournament));
            }
            else
            {
                //If i am here and the number of player is odd, that means soemthing is off
                throw new Exception("Number of player is odd");
            }
        }

        //shuffle the matches to distribute the byes matches evenly.
        //matches is saved by the order of the match creation, so i need to shuffle it
        var random = new Random();
        return matches.OrderBy(m => random.Next()).ToList();
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

    private Round GetRound(int roundNumber, MatchType matchType, int playerCount)
    {
        var roundName = GetRoundName(playerCount);
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
        return HighestPowerof2Ceil(count) - count;
    }

    private int HighestPowerof2Ceil(int N)
    {
        // if N is a power of two simply return it
        if ((N & (N - 1)) == 0)
            return N;
        // else set only the most significant bit
        return 1 << (Convert.ToString(N, 2).Length);
    }

    private async Task<Player> GetByePlayer()
    {
        if (_byePlayer == null)
        {
            //search db if already exists
            var result = (await _playerRepository.GetAllAsync(p => p.Email == Utility.ByePlayerEmail)).FirstOrDefault();
            if (result != null)
            {
                _byePlayer = result;
            }
            else
            {
                _byePlayer = new Player
                {
                    //One bye player for the entire Tournament platform
                    Name = Utility.ByePlayerName,
                    Email = Utility.ByePlayerEmail,
                    Age = 18,
                };
            }
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
            -1 => "Playoff 3/4", //-1 is for playoff, selected randomly
            _ => "KnockOut"
        };
    }
    public async Task<IEnumerable<Match>> CreateSubsequentMatches(Tournament tournament, MatchType matchType)
    {
        ArgumentNullException.ThrowIfNull(matchType);
        ArgumentNullException.ThrowIfNull(tournament);

        //get the latest previous round matches
        var previousRound = matchType.Rounds.OrderBy(r => r.RoundNumber).LastOrDefault();
        if (previousRound == null) throw new Exception("Previous round can not be null");

        //populate the matches of the previous round
        if( previousRound.Matches == null || previousRound.Matches.Count == 0)
        {
            await _roundRepository.ExplicitLoadCollectionAsync(previousRound, r => r.Matches);
        }

        //still if previous round match count is 0 then throw an exception
        if(previousRound!.Matches!.Count == 0)
        {
            throw new Exception("Previous round matches can not be empty");
        }

        //get the winner of the previous round matches
        //! Need to check the order of the player
        // say AvsB and CvsD and EvsF and GvsH, then AvsC and EvsG, it should alternate

        var winners = previousRound.Matches.OrderBy(mt => mt.Id).Select(m => m.Winner).Distinct().ToList();

        //make match for the current round
        var currentRound = GetRound(previousRound.RoundNumber + 1, matchType, winners.Count);


        //check if the number of winners is even
        if (winners.Count % 2 != 0)
        {
            //at this point, we should have even number of players, coz already handle the odd number of player in the first round
            throw new Exception("Number of winners is odd");
        }

        List<Match> matches = new List<Match>();

        //make playoff for third and fourth place
        if(currentRound.RoundName == "Final")
        {
            //get the loser of the semifinal
            var semiFinalLosers = previousRound.Matches.Select(mt => mt.Winner == mt.SecondPlayer ? mt.FirstPlayer : mt.SecondPlayer).ToList();
            var playoffRound = GetRound(previousRound.RoundNumber + 2, matchType, -1); // palyouff round with index -1
            matches.Add(GetMatch(semiFinalLosers[0], semiFinalLosers[1], playoffRound, tournament));
        }

        for (int i = 0; i < winners.Count; i+=2)
        {
            var firstPlayer = winners[i];   
            var secondPlayer = winners[i+1];
            if(firstPlayer == null || secondPlayer == null)
            {
                throw new Exception("Player can not be null");
            }
            matches.Add(GetMatch(firstPlayer, secondPlayer, currentRound, tournament));
        }

        return matches;
    }
}
