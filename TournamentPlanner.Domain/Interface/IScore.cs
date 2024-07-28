using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentPlanner.Domain.Interface
{
    public interface IScore
    {
        bool IsComplete { get; }
    }
}