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
    public class RoundRepository : IRepository<Round, Round>
    {
        private readonly TournamentPlannerDataContext _dataContext;
        public RoundRepository(TournamentPlannerDataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<Round> AddAsync(Round obj)
        {
            _dataContext.Rounds.Add(obj);
            await Task.CompletedTask;
            return obj;
        }

        public async Task<IEnumerable<Round>> GetAllAsync()
        {
            return await _dataContext.Rounds.ToListAsync();
        }
        //TODO: add this method to interface
        public async Task<IEnumerable<Round>> GetAllAsync(string[]? includeProperties = null)
        {
            var query = _dataContext.Rounds;

            if(includeProperties != null){
                foreach (var property in includeProperties){
                    query = (DbSet<Round>)query.Include(r => r.GetType().GetProperty(property));
                }
            }
            return await query.ToListAsync();
        }

        public Task<IEnumerable<Round>> GetAllAsync(Func<Round, bool> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Round>> GetAllAsync(Func<Round, bool> filter, string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Round>> GetAllAsync(IEnumerable<Func<Round, bool>> filters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Round>> GetAllAsync(IEnumerable<Func<Round, object>> includeByExpression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Round>> GetAllAsync(IEnumerable<Func<Round, object>> includeByExpression, IEnumerable<Func<Round, bool>> filters)
        {
            throw new NotImplementedException();
        }

        public async Task<Round?> GetByIdAsync(int id)
        {
            return await _dataContext.Rounds.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<IEnumerable<Round>?> GetByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public Task<Round> UpdateAsync(Round obj)
        {
            throw new NotImplementedException();
        }

        public Task<Round> UpdateByIdAsync(int id, Round obj)
        {
            throw new NotImplementedException();
        }
    }
}