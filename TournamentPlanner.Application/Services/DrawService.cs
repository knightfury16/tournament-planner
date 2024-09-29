using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;

namespace TournamentPlanner.Application;

public interface IDrawService
{
    public Task<bool> IsTheDrawComplete(Tournament tournament);
    public Task<bool> IsTheDrawComplete(IEnumerable<Draw> draws);
}
public class DrawService : IDrawService
{
    private readonly IRepository<Draw> _drawRepository;

    public DrawService(IRepository<Draw> drawRepository)
    {
        _drawRepository = drawRepository;
    }

    public async Task<bool> IsTheDrawComplete(Tournament tournament)
    {
        var draws = await _drawRepository.GetAllAsync(d => d.TournamentId == tournament.Id, [nameof(Draw.MatchType)]);

        foreach (var draw in draws)
        {
            if (draw.MatchType.IsCompleted == false) return false;
        }
        return true;
    }

    public Task<bool> IsTheDrawComplete(IEnumerable<Draw> draws)
    {
        if(draws == null) throw new ArgumentNullException(nameof(draws));

        foreach (var draw in draws)
        {
            if (draw.MatchType == null) throw new NotFoundException(nameof(draw.MatchType));
            if (draw.MatchType.IsCompleted == false) return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
}
