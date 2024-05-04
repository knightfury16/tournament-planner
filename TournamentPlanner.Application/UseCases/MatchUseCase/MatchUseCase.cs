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
        public async Task<IEnumerable<Match>> GetAllMatches(int? roundId)
        {
            var matches = await _matchRepository.GetAllAsync();

            if (roundId != null)
            {
                matches = matches.Where(match => match.Round.RoundNumber == roundId);
            }
            return matches;
        }

        public async Task<IEnumerable<Player?>?> GetAllWinnersOfRound(int roundId)
        {
            var matches = await _matchRepository.GetAllAsync();
            var winners = matches.Where(match => match.Round.RoundNumber == roundId)
                                        .Where(match => match.IsComplete == true)
                                        .Select(match => match.Winner);
            return winners;
        }

        public async Task<IEnumerable<Match>> GetOpenMatches(int? roundId)
        {
            var matches = await _matchRepository.GetAllAsync();

            if (roundId != null)
            {
                matches = matches.Where(match => match.Round.RoundNumber == roundId);
            }

            matches = matches.Where(match => match.IsComplete == false);

            return matches;
        }

        public async Task<IEnumerable<Match>> GetPlayedMatches(int? roundId)
        {
            var matches = await _matchRepository.GetAllAsync();

            if (roundId != null)
            {
                matches = matches.Where(match => match.Round.RoundNumber == roundId);
            }

            matches = matches.Where(match => match.IsComplete == true);

            return matches;
        }

        public async Task<Player?> GetWinnerOfMatch(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match is null){
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

                return m;
            }

            else
            {
                throw new Exception("Problem in scheduling");
            }

        }
    }
}