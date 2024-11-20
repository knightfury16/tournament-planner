namespace TournamentPlanner.Test.Application.RequestHandlers
{
    using Xunit;
    using Moq;
    using AutoMapper;
    using TournamentPlanner.Application;
    using TournamentPlanner.Application.Common.Interfaces;
    using TournamentPlanner.Application.DTOs;
    using TournamentPlanner.Domain.Entities;
    using TournamentPlanner.Domain.Exceptions;
    using System.Threading;
    using System.Collections.Generic;
    using System.Linq;
    using TournamentPlanner.Test.Fixtures;
    using System.Linq.Expressions;

    public class GetTournamentMatchesTypesRequestHandlerTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRepository<Draw>> _drawRepository;
        private readonly GetTournamentMatchesTypesRequestHandler _handler;

        public GetTournamentMatchesTypesRequestHandlerTest()
        {
            _mapper = new Mock<IMapper>();
            _drawRepository = new Mock<IRepository<Draw>>();
            _handler = new GetTournamentMatchesTypesRequestHandler(_drawRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsTournamentMatchesTypes()
        {
            // Arrange
            var tournament = TournamentFixtures.GetTournament();
            var matchType = MatchTypeFixtures.GetGroup();
            var draw = new Draw { MatchType = matchType, Tournament = tournament };
            var expectedMatchTypes = new List<MatchTypeDto> { new MatchTypeDto { Name = matchType.Name } };

            _drawRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Draw, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Draw> { draw });
            _mapper.Setup(m => m.Map<IEnumerable<MatchTypeDto>>(It.IsAny<IEnumerable<MatchType>>())).Returns(expectedMatchTypes);

            var request = new GetTournamentMatchTypesRequest(draw.TournamentId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedMatchTypes, result);
        }

        [Fact]
        public async Task Handle_DrawIsNull_ThrowsException()
        {
            // Arrange
            var request = new GetTournamentMatchTypesRequest(1);

            _drawRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Draw, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Draw>());

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}

