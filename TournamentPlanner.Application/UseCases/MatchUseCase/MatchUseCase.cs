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
        public async Task<IEnumerable<Match>> GetAllRoundMatches(int roundId)
        {

            //without player there is no use of matches. So include player on deafult match fetch 
            return await _matchRepository.GetAllAsync(match => match.RoundId == roundId, ["FirstPlayer", "SecondPlayer"]);
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            //without player there is no use of matches. So include player on deafult match fetch 
            return await _matchRepository.GetAllAsync(["FirstPlayer", "SecondPlayer"]);
        }
        public async Task<IEnumerable<Match>> GetAllTournamentMatches(int tournamentId)
        {
            //without player there is no use of matches. So include player on deafult match fetch 
            return await _matchRepository.GetAllAsync(match => match.Round?.TournamentId == tournamentId,["FirstPlayer", "SecondPlayer", "Round.Tournament"]);
        }

        public async Task<IEnumerable<Player?>?> GetAllWinnersOfRound(int roundId)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.RoundId == roundId && match.IsComplete == true, ["Winner"]);

            var winners = matches.Select(match => match.Winner);
            return winners;
        }

        public async Task<IEnumerable<Match>> GetOpenMatches(int? roundId, string? tournamentName)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.IsComplete == false, ["FirstPlayer", "SecondPlayer", "Round.Tournament"]);

            if (roundId.HasValue)
            {
                matches = matches.Where(match => match.RoundId == roundId);
            }

            if (!string.IsNullOrEmpty(tournamentName))
            {
                matches = matches.Where(match => match.Round?.Tournament?.Name == tournamentName);
            }

            return matches;
        }

        public async Task<IEnumerable<Match>> GetPlayedMatches(int? roundId, string? tournamentName)
        {
            var matches = await _matchRepository.GetAllAsync(match => match.IsComplete == true, ["FirstPlayer", "SecondPlayer", "Round.Tournament"]);

            if (roundId.HasValue)
            {
                matches = matches.Where(match => match.RoundId == roundId);
            }

            if (!string.IsNullOrEmpty(tournamentName))
            {
                matches = matches.Where(match => match.Round?.Tournament?.Name == tournamentName);
            }

            return matches;
        }

        public async Task<Player?> GetWinnerOfMatch(int matchId)
        {
            //TODO: GetById should have an overload for include properties
            // var match = await _matchRepository.GetByIdAsync(matchId);
            var match = await _matchRepository.GetAllAsync(m => m.Id == matchId, ["Winner"]);
            if (match is null)
            {
                throw new Exception("Match not found");
            }
            if (match.First().IsComplete)
            {
                return match.First().Winner;
            }
            return null;
        }

        public async Task<Match> RescheduleAMatch(int matchId, DateTime rescheduledDate)
        {
            //TODO: Add some logic, like if game is already played or if any game is being played on that day or not
            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match != null)
            {
                match.GameScheduled = rescheduledDate;

                //No need to call update method, coz it is being tracked by ef
                // await _matchRepository.UpdateAsync(m);
                await _matchRepository.SaveAsync();
                return match;
            }

            else
            {
                throw new Exception("Problem in scheduling");
            }

        }
    }
}