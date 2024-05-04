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
    public class MatchRepository : IRepository<Match, Match>
    {
        private readonly TournamentPlannerDataContext _dataContext;
        public MatchRepository(TournamentPlannerDataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public Task<Match> AddAsync(Match obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            var matches = await _dataContext.Matches.AsNoTracking()
            .Include(match => match.Winner)
            .Include(match => match.FirstPlayer)
            .Include(match => match.SecondPlayer)
            .Include(nameof(Match.Round) + "." + nameof(Round.Tournament))
            .ToListAsync();
            return matches;
        }

        public async Task<Match> GetByIdAsync(int id)
        {
            var match = await _dataContext.Matches.Include(match => match.Winner).FirstOrDefaultAsync(match => match.Id == id);

            if (match == null)
            {
                throw new Exception("Match could not be found");
            }
            return match;
        }

        public Task<IEnumerable<Match>> GetByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Match> UpdateAsync(Match obj)
        {
            _dataContext.Attach(obj).State = EntityState.Modified;
            await Task.CompletedTask;
            // await _dataContext.SaveChangesAsync();
            return obj;
        }

        public Task<Match> UpdateByIdAsync(int id, Match obj)
        {
            throw new NotImplementedException();
        }
    }
}