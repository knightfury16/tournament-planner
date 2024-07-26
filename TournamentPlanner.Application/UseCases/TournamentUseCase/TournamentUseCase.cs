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
        private readonly IRepository<Player, Player> _playerRepository;

        public TournamentUseCase(IRepository<Tournament, Tournament> tournamentRepository, IRepository<Player, Player> playerRepository)
        {
            _tournamentRepository = tournamentRepository;
            _playerRepository = playerRepository;
        }
        public async Task<Tournament> AddTournamnet(TournamentDto tournamentDto)
        {
            var tournament = MapDtoToTournament(tournamentDto);
            await _tournamentRepository.AddAsync(tournament);
            await _tournamentRepository.SaveAsync();
            return tournament;
        }

        public async Task<IEnumerable<Tournament>> GetAll(string? name, DateOnly? startDate, DateOnly? endDate)
        {
            List<Func<Tournament, bool>> filters = new();

            if(!string.IsNullOrEmpty(name)){
                filters.Add(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if(startDate.HasValue){
                filters.Add(x => x.StartDate?.Date >= startDate.Value.ToDateTime(TimeOnly.MinValue));
            }
            if(endDate.HasValue){
                filters.Add(x => x.EndDate?.Date <= endDate.Value.ToDateTime(TimeOnly.MinValue));
            }

            var tournaments = await _tournamentRepository.GetAllAsync(filters);

            return tournaments;
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

        public async Task<Tournament?> GetTournamentbyId(int id)
        {
            //!! Will be changed 
            //* after freeing the player of tournament, i can remove the jsonignore property and can directly populate the players in one call. Might not also need the tournamentResponseDto
            var tournament = await _tournamentRepository.GetByIdAsync(id);
            //if no tournament found return
            if(tournament == null)return null;

            //fetch all the players playing in the tournament
            var players = await _playerRepository.GetAllAsync( player => player.TournamentId == id);

            tournament.Players = (List<Player>)players;

            return tournament;

             // return await _tournamentRepository.GetByIdAsync(id);

        }
    }
}
