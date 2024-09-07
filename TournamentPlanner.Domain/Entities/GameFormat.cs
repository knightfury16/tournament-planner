using System.Text.Json;
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

        public abstract IScore DeserializeScore(object scoreData);
    }
}