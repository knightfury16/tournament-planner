using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminCreatedTournamentsRequest : IRequest<List<TournamentDto>>
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => _name = value?.ToLower();
    }
    public TournamentSearchCategory SearchCategory { get; set; } = TournamentSearchCategory.All;
    public TournamentStatus? Status { get; set; }
    public GameTypeSupported? GameTypeSupported { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
