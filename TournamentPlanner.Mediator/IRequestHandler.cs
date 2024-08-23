using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentPlanner.Mediator
{
    public interface IRequestHandler<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        //Why did i made TResponse nullable? If find answe right here
        Task<TResponse?> Handle(TRequest request, CancellationToken cancellationToken1 = default);
    }
}