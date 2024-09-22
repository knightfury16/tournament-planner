using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICreateMatch
{
    Task<IEnumerable<Match>> CreateMatches(Tournament tournament, MatchType matchType);
}
