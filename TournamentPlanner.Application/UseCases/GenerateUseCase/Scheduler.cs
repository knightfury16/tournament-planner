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
            
            if(round.StartTime is null){
                throw new ArgumentException("Cant not schedule match. Round start time is not specified");
            }

            var roundStartTimeUtc = round.StartTime.Value.ToUniversalTime();

            //staring match after two hour of round start
            var matchStartTimeUtc = roundStartTimeUtc.Date.AddHours(10); //from 10 in the morning

            // Adjust match start time if round start time is after 10am UTC
            if(roundStartTimeUtc.TimeOfDay > new TimeSpan(10,0,0)){

                //start match one hour after round start time if its before 5pm
                if(roundStartTimeUtc.TimeOfDay < new TimeSpan(17,0,0)){
                    matchStartTimeUtc = roundStartTimeUtc.AddHours(1).ToUniversalTime();
                }
                else{
                    // start the next day
                    matchStartTimeUtc = roundStartTimeUtc.AddDays(1).AddHours(10).ToUniversalTime();
                }
            }

            var matchScheduleOnThisDate = 0;

            foreach (var match in matches)
            {
                if(matchScheduleOnThisDate == MAX_MATCH_EACH_DAY || matchStartTimeUtc.TimeOfDay > new TimeSpan(17,0,0)){
                    matchStartTimeUtc = matchStartTimeUtc.Date.AddDays(1).AddHours(10).ToUniversalTime();
                    matchScheduleOnThisDate = 0;
                }
                match.GameScheduled = matchStartTimeUtc;
                matchScheduleOnThisDate++;
                //TODO: Configure break time dynamically
                matchStartTimeUtc = matchStartTimeUtc.AddHours(2).ToUniversalTime(); // match duration 1 hour + 1 hour break after each match
            }
            return matches;
        }
    }
}