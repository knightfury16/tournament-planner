using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class CreateMatchTypeRequestHandler : IRequestHandler<CreateMatchTypeRequest, IEnumerable<MatchType>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ICreateMatchTypeFactory _createMatchTypeFactory;
    public CreateMatchTypeRequestHandler(IRepository<Tournament> tournamentRepository, ICreateMatchTypeFactory createMatchTypeFactory)
    {
        _tournamentRepository = tournamentRepository;
        _createMatchTypeFactory = createMatchTypeFactory;
    }


    public async Task<IEnumerable<MatchType>?> Handle(CreateMatchTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Participants)])).FirstOrDefault();

        if (tournament == null) throw new Exception("Tournament not found");

        if (tournament.Status == TournamentStatus.Ongoing || tournament.StartDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Can not make match type");
        }
        if (tournament.Participants.Count == 0)
        {
            throw new Exception("Can not create match type with no participant");
        }

        var matchTypeCreator = _createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);

        var matchTypes = await matchTypeCreator.CreateMatchType(tournament);

        if (matchTypes == null)
        {
            throw new Exception("Could not create match types. see inner exception");
        }

        return matchTypes;
    }
}
