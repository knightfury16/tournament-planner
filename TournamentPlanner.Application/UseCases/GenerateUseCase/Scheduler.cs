using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.GenerateUseCase
{
    public class Scheduler
    {
        public static int MAX_MATCH_EACH_DAY { get; set; } = 4;
        public static List<Match> Schedule(List<Match> matches, Round round){
            var roundStartTime = round.StartTime;
            
            if(roundStartTime is null){
                throw new ArgumentException("Cant not schedule match. Round start time is not specified");
            }

            //staring match after two hour of round start
            var matchStartTime = roundStartTime.Value.Date.AddHours(10); //from 10 in the morning

            var matchScheduleOnThisDate = 0;

            foreach (var match in matches)
            {
                if(matchScheduleOnThisDate == MAX_MATCH_EACH_DAY){
                    matchStartTime = matchStartTime.Date.AddDays(1).AddHours(10);
                    matchScheduleOnThisDate = 0;
                }
                match.GameScheduled = matchStartTime;
                matchScheduleOnThisDate++;
                matchStartTime = matchStartTime.AddMinutes(150); // 1.5 hour gap
            }

            return matches;
        }
    }
}