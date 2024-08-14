namespace TournamentPlanner.Application.DTOs
{
    public class AdminDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public List<TournamentDto>? CreatedTournament { get; set; }
        
    }
}