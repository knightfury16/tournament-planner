using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Infrastructure.DataContext;

namespace TournamentPlanner.Infrastructure
{
    public class PlayerRepository : IRepository<PlayerDto, Player>
    {
        public TournamentPlannerDataContext _context { get; }

        public PlayerRepository(TournamentPlannerDataContext context)
        {
            _context = context;

        }

        public async Task<Player> AddAsync(PlayerDto obj)
        {
            var player = FromDto(obj);
            _context.Players.Add(player);
            // await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return player;
        }


        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Player> UpdateAsync(PlayerDto obj)
        {
            throw new NotImplementedException();
        }

        public async Task<Player> UpdateByIdAsync(int id, PlayerDto obj)
        {
            var player = await _context.Players.FindAsync(id);

            if(player is null){
                throw new ArgumentException("Player not found");
            }

            player.Name = obj.Name;
            player.PhoneNumber = obj.PhoneNumber;
            player.Email = obj.Email;
            
            // await _context.SaveChangesAsync();

            return player;
        }

        public PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email
            };
        }

        public Player FromDto(PlayerDto dto)
        {
            return new Player
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _context.Players.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Player>?> GetByNameAsync(string? name)
        {
            if(string.IsNullOrEmpty(name)){
                return await _context.Players.AsNoTracking().ToListAsync();
            }

            return await _context.Players.AsNoTracking().Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}