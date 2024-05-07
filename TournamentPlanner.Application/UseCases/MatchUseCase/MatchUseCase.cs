using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.MatchUseCase
{
    public class MatchUseCase : IMatchUseCase
    {
        private IRepository<Match, Match> _matchRepository;
        public MatchUseCase(IRepository<Match, Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }
        public async Task<IEnumerable<Match>> GetAllMatches(int roundId)
        {
            return  await _matchRepository.GetAllAsync(match => match.RoundId == roundId);
        }

        public async Task<IEnumerable<Player?>?> GetAllWinnersOfRound(int roundId)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.Round?.RoundNumber == roundId && match.IsComplete == true, ["Winner"]);

            var winners = matches.Select(match => match.Winner);
            return winners;
        }

        public async Task<IEnumerable<Match>> GetOpenMatches(int? roundId)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.IsComplete == false);

            if (roundId != null)
            {
                matches = matches.Where(match => match.RoundId == roundId);
            }

            return matches;
        }

        public async Task<IEnumerable<Match>> GetPlayedMatches(int? roundId)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.IsComplete == true);

            if (roundId != null)
            {
                matches = matches.Where(match => match.Round?.RoundNumber == roundId);
            }

            return matches;
        }

        public async Task<Player?> GetWinnerOfMatch(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match is null)
            {
                throw new Exception("Match not found");
            }
            return match.Winner;
        }

        public async Task<Match> RescheduleAMatch(int matchId, DateOnly rescheduledDate)
        {
            var m = await _matchRepository.GetByIdAsync(matchId);

            if (m != null)
            {
                m.GameScheduled = rescheduledDate;

                await _matchRepository.UpdateAsync(m);
                await _matchRepository.SaveAsync();

                return m;
            }

            else
            {
                throw new Exception("Problem in scheduling");
            }

        }
    }
}