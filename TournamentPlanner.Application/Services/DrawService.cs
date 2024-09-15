using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application;

public interface IDrawService
{
    public Task<bool> IsTheDrawComplete(Tournament tournament);
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

}
