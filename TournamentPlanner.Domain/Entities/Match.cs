using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class Match<TScore> : BaseEntity where TScore : IScore
    {
        public required Player FirstPlayer { get; set; }
        public required Player SecondPlayer { get; set; }

        public Player? Winner { get; set; }

        public DateTime? GameScheduled { get; set; }

        public DateTime? GamePlayed { get; set; }

        public TScore? Score { get; set; }

    }
}