using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.DataSeeder
{
    public class PlayerBuilder
    {
        private Player _player = new Player()
        {
            Name = "user",
            Email = "default@gmail.com",
        };

        public PlayerBuilder WithName(string name)
        {
            _player.Name = name;
            return this;
        }

        public PlayerBuilder WithEmail(string email)
        {
            _player.Email = email;
            return this;
        }

        public PlayerBuilder WithAge(int age)
        {
            _player.Age = age;
            return this;
        }

        public PlayerBuilder WithWeight(int weight)
        {
            _player.Weight = weight;
            return this;
        }

        public Player Build()
        {
            _player.CreatedAt = DateTime.UtcNow;
            _player.UpdatedAt = DateTime.UtcNow;
            return _player;
        }
    }
}