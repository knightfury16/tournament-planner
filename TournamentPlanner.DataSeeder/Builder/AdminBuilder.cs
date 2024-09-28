using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.DataSeeder
{
    public class AdminBuilder
    {
        private Admin _admin = new Admin()
        {
            Name = "user",
            Email = "default@gmail.com",
            PhoneNumber = "12345"
        };

        public AdminBuilder WithName(string name)
        {
            _admin.Name = name;
            return this;
        }

        public AdminBuilder WithEmail(string email)
        {
            _admin.Email = email;
            return this;
        }

        public AdminBuilder WithPhoneNumber(string phoneNumber)
        {
            _admin.PhoneNumber = phoneNumber;
            return this;
        }

        public Admin Build()
        {
            _admin.CreatedAt = DateTime.UtcNow;
            _admin.UpdatedAt = DateTime.UtcNow;
            return _admin;
        }

    }
}