using System.Text.Json;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public abstract class GameFormat
    {
        public JsonSerializerOptions JsonOptions { get; }

        protected GameFormat(JsonSerializerOptions? jsonOptions = null)
        {
            JsonOptions = jsonOptions ?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public abstract IScore CreateInitialScore();
        public abstract bool IsValidScore(IScore score);
        public abstract Player DetermineWinner(Player player1, Player player2, IScore score);

        public virtual string SerializeScore(object score)
        {
            return JsonSerializer.Serialize(score);
        }

        //Deafult group standing calculation based on only number of game wins, then by match points
        public virtual List<PlayerStanding> GetGroupStanding(Tournament tournament, MatchType matchType, bool completeStanding = false)
        {
            if (tournament == null || matchType == null) throw new ArgumentNullException(nameof(GetGroupStanding));

            var winnerPerGroup = tournament.WinnerPerGroup;

            var playerStandings = matchType.Players.Select(p => new PlayerStanding { Player = p }).ToDictionary(ps => ps.Player.Id);

            foreach (var round in matchType.Rounds)
            {
                foreach (var match in round.Matches)
                {
                    if (match.Winner == null) throw new Exception("Winner not found");
                    var winnerStanding = playerStandings[match.Winner.Id];
                    var loserStanding = playerStandings[match.Winner.Id == match.FirstPlayer.Id ? match.SecondPlayer.Id : match.FirstPlayer.Id];

                    winnerStanding.Wins++;
                    winnerStanding.MatchPoints += 2;
                    loserStanding.Losses++;
                    loserStanding.MatchPoints += 1;
                }
            }

            return completeStanding ? playerStandings.OrderByDescending(ps => ps.Wins).ThenBy(ps => ps.MatchPoints).ToList() : playerStandings.OrderByDescending(ps => ps.Wins).ThenBy(ps => ps.MatchPoints).Take(winnerPerGroup).ToList();
        }

        public abstract IScore DeserializeScore(object scoreData);
    }
}