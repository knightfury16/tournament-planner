using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.TournamentUseCase
{
    public class TournamentUseCase : ITournamentUseCase
    {
        private readonly IRepository<Tournament, Tournament> _tournamentRepository;

        public TournamentUseCase(IRepository<Tournament, Tournament> tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }
        public async Task<Tournament> AddTournamnet(TournamentDto tournamentDto)
        {
            var tournament = MapDtoToTournament(tournamentDto);
            await _tournamentRepository.AddAsync(tournament);
            await _tournamentRepository.SaveAsync();
            return tournament;
        }

        public async Task<IEnumerable<Tournament>> GetAll()
        {
            return await _tournamentRepository.GetAllAsync();
        }

        public Task<IEnumerable<Tournament>> GetTournamentsByDate(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }



        public TournamentDto MapTournamentToDto(Tournament tournament)
        {
            return new TournamentDto
            {
                Name = tournament.Name,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate
            };
        }

        public Tournament MapDtoToTournament(TournamentDto tournamentDto)
        {
            return new Tournament
            {
                Name = tournamentDto.Name,
                StartDate = tournamentDto.StartDate,
                EndDate = tournamentDto.EndDate
            };
        }
    }
}
