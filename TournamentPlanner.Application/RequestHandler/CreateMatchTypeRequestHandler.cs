using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class CreateMatchTypeRequestHandler : IRequestHandler<CreateMatchTypeRequest, IEnumerable<MatchTypeDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ICreateMatchTypeFactory _createMatchTypeFactory;
    private readonly IMapper _mapper;
    public CreateMatchTypeRequestHandler(IRepository<Tournament> tournamentRepository, ICreateMatchTypeFactory createMatchTypeFactory, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _createMatchTypeFactory = createMatchTypeFactory;
        _mapper = mapper;
    }


    public async Task<IEnumerable<MatchTypeDto>?> Handle(CreateMatchTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Participants)])).FirstOrDefault();

        if (tournament == null) throw new NotFoundException(nameof(tournament));
        //!!On Test
        //if (tournament.Status == TournamentStatus.Ongoing || tournament.StartDate < DateTime.UtcNow)
        //{
        //    throw new ValidationException("Can not make match type. check Tournament status and start date.");
        //}
        if (tournament.Participants.Count == 0)
        {
            throw new ValidationException("Can not create match type with no participant");
        }

        var matchTypeCreator = _createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);

        var matchTypes = await matchTypeCreator.CreateMatchType(tournament,"Cool");

        if (matchTypes == null)
        {
            throw new Exception("Could not create match types. see inner exception");
        }
        //not saving it in the db yet
        //!!ON TEST

        return _mapper.Map<IEnumerable<MatchTypeDto>>(matchTypes);
    }
}
