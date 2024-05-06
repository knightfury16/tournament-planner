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

        public Task<IEnumerable<Tournament>> GetAllAsync(Func<Tournament, bool> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync(string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync(Func<Tournament, bool> filter, string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync(IEnumerable<Func<Tournament, bool>> filters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync(IEnumerable<Func<Tournament, object>> includeByExpression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync(IEnumerable<Func<Tournament, object>> includeByExpression, IEnumerable<Func<Tournament, bool>> filters)
        {
            throw new NotImplementedException();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tournament>?> GetByNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                return await _context.Tournaments.ToListAsync();
            }

            return await _context.Tournaments.Where(t => t.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
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