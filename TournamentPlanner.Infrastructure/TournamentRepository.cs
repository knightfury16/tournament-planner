using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Infrastructure.DataContext;

namespace TournamentPlanner.Infrastructure
{
    public class TournamentRepository : IRepository<Tournament, Tournament>
    {
        private readonly TournamentPlannerDataContext _context;

        public TournamentRepository(TournamentPlannerDataContext context)
        {
            _context = context;
        }

        public async Task<Tournament> AddAsync(Tournament obj)
        {
            _context.Tournaments.Add(obj);
            await Task.CompletedTask;
            return obj;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await _context.Tournaments.ToListAsync();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<IEnumerable<Tournament>?> GetByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tournament> UpdateAsync(Tournament obj)
        {
            throw new NotImplementedException();
        }

        public Task<Tournament> UpdateByIdAsync(int id, Tournament obj)
        {
            throw new NotImplementedException();
        }
    }
}