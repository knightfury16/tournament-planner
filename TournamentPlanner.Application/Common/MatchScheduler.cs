
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application;

public interface IMatchScheduler
{
    public Task<IEnumerable<Match>> DefaultMatchScheduler(List<Match> matches, SchedulingInfo schedulingInfo);
    public Task<IEnumerable<Match>> AdvanceMatchScheduler(List<Match> matches, SchedulingInfo schedulingInfo);
}
public class MatchScheduler : IMatchScheduler
{

    public IEnumerable<Match> DefaultMatchScheduler(List<Match> matches, SchedulingInfo schedulingInfo)
    {
        var tournament = matches.First().Tournament; // do i need to send the tournament explicitly here?

        // if start time is provided or else default to 30 minutes later of touranament start date
        TimeOnly startTime = schedulingInfo?.StartTime ?? new TimeOnly(10, 0, 0); //10am
        var eachMatchTime = schedulingInfo?.EachMatchTime ?? TimeSpan.FromMinutes(30);
        DateTime modifiedStartDate = GetModifiedStartDate(startTime, schedulingInfo?.StartDate ?? tournament.StartDate);

        foreach (var match in matches)
        {
            match.Duration = eachMatchTime;
            match.GameScheduled = modifiedStartDate;
            modifiedStartDate = modifiedStartDate.AddMinutes(30);

            //if start time is greater than 7pm go to next day
            if (modifiedStartDate.Hour >= 19)
            {
                modifiedStartDate = modifiedStartDate.AddDays(1).Date.AddHours(10); //next day 10am
            }
        }

        return matches;
    }

    private DateTime GetModifiedStartDate(TimeOnly startTime, DateTime dateTime)
    {
        int hour = startTime.Hour;
        int minute = startTime.Minute;
        int second = startTime.Second;

        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second).ToUniversalTime(); // setting the start time with start date
    }

    public IEnumerable<Match> AdvanceMatchScheduler( List<Match> matches, SchedulingInfo schedulingInfo)
    {
        //a scheduler that takes into accout the following things
        //1. match per day -> end day - start day = day we have to finish all the matches. 
        //if match per day is defined we have to calculate if it is possible to finish the tournament within the available days 
        //of the tournament. for that we have to calcualte the total possible match that can be played with in 
        //the tournament. 
        //2. parallelism -> how many matches can be played at once. will be pessimist here and make the default 1
        //when calculating if it is possible to finish all the matches with the given the match per day, will take this parameter 
        // into accoutn too.
        //3. match start time -> will be constant for all the matches within the tournament unless it is being change.
        //4. time per match -> will also have to take this parameter into account when calculating if is possible the
        //finish the tournament with in the alotted time.
        //remember to keep in mind when writting the rescheduler method, to check the collision
        throw new NotImplementedException();
    }
}
