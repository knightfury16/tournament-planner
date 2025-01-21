
using TournamentPlanner.Application.Common.Interfaces;
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
    private readonly IRepository<Match> _matchRepository;

    public MatchScheduler(IRepository<Match> matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<IEnumerable<Match>> DefaultMatchScheduler(List<Match> matches, SchedulingInfo schedulingInfo)
    {
        ArgumentNullException.ThrowIfNull(matches);

        await PopulateTournament(matches.First());

        var tournament = matches.First().Tournament;

        // if start time is provided or else default to 30 minutes later of touranament start date
        TimeOnly startTime = GetStartTime(schedulingInfo.StartTime);
        TimeOnly endTime = GetEndTime(schedulingInfo.EndTime);
        var eachMatchTime = GetEachMatchTime(schedulingInfo.EachMatchTime);
        DateTime modifiedStartDate = GetModifiedStartDate(startTime, schedulingInfo?.StartDate ?? tournament.StartDate);

        foreach (var match in matches)
        {
            match.Duration = eachMatchTime;
            match.GameScheduled = modifiedStartDate;
            modifiedStartDate = modifiedStartDate.AddMinutes(eachMatchTime.TotalMinutes);

            //if start time is greater than endtime go to next day
            if (modifiedStartDate.Hour >= endTime.Hour)
            {
                modifiedStartDate = GetModifiedStartDate(startTime, modifiedStartDate.AddDays(1));
            }
        }

        return matches;
    }

    private TimeSpan GetEachMatchTime(string? eachMatchTime)
    {
        var defaultTimeSpanOfMatchTime = SchedulingInfo.DefaultMatchDuration;
        return eachMatchTime != null ? ConvertToTimeSpan(eachMatchTime) : defaultTimeSpanOfMatchTime;
    }

    private TimeOnly GetStartTime(string? startTime)
    {
        var defaultStartTime = SchedulingInfo.DefaultStartTime; //10am
        return startTime != null ? ConvertToTimeOnly(startTime) : defaultStartTime;
    }
    private TimeOnly GetEndTime(string? endTime)
    {
        var defaultEndTime = SchedulingInfo.DefaultEndTime;
        return endTime != null ? ConvertToTimeOnly(endTime) : defaultEndTime;
    }

    private TimeOnly ConvertToTimeOnly(string startTime)
    {
        var success = TimeOnly.TryParse(startTime, out var startTimeParsed);
        if (!success) throw new InvalidOperationException("Can not parse timeonly from given Time string");
        return startTimeParsed;
    }

    private TimeSpan ConvertToTimeSpan(string eachMatchTime)
    {
        var success = int.TryParse(eachMatchTime, out var eachMatchTimeInt);
        if (!success) throw new InvalidOperationException("Can not parse timespan from Each Match Time");
        return TimeSpan.FromMinutes(eachMatchTimeInt);
    }

    private async Task PopulateTournament(Match match)
    {
        //load tournament if null
        if (match.Tournament != null) return;
        await _matchRepository.ExplicitLoadReferenceAsync(match, m => m.Tournament);
    }

    private DateTime GetModifiedStartDate(TimeOnly startTime, DateTime dateTime)
    {
        int hour = startTime.Hour;
        int minute = startTime.Minute;
        int second = startTime.Second;

        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second); // setting the start time with start date
    }

    public async Task<IEnumerable<Match>> AdvanceMatchScheduler(List<Match> matches, SchedulingInfo schedulingInfo)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentNullException.ThrowIfNull(schedulingInfo);

        await PopulateTournament(matches.First());

        var tournament = matches.First().Tournament;
        DateTime startDate = schedulingInfo.StartDate ?? tournament.StartDate;
        TimeOnly startTime = GetStartTime(schedulingInfo.StartTime);
        TimeOnly endTime = GetEndTime(schedulingInfo.EndTime);
        TimeSpan eachMatchTime = GetEachMatchTime(schedulingInfo.EachMatchTime);
        int matchPerDay = schedulingInfo.MatchPerDay > 0 ? schedulingInfo.MatchPerDay : 10; // Default to 10 matches per day
        int parallelMatches = schedulingInfo.ParallelMatchesPossible > 0 ? schedulingInfo.ParallelMatchesPossible : 1;

        DateTime currentDateTime = GetModifiedStartDate(startTime, startDate);
        int totalDays = (tournament.EndDate.HasValue ? (tournament.EndDate.Value - startDate).Days : 7) + 1; // Default to 7 days if no end date

        int totalMatches = matches.Count;
        int totalPossibleMatches = totalDays * matchPerDay * parallelMatches;

        if (totalMatches > totalPossibleMatches)
        {
            int extraDaysNeeded = (int)Math.Ceiling((double)(totalMatches - totalPossibleMatches) / (matchPerDay * parallelMatches));
            throw new InvalidOperationException($"Not enough days to schedule all matches with the given constraints. You need {extraDaysNeeded} more day(s) to complete the schedule.");
        }

        int matchesScheduledToday = 0;
        for (int i = 0; i < matches.Count; i += parallelMatches)
        {
            for (int j = 0; j < parallelMatches && i + j < matches.Count; j++)
            {
                var match = matches[i + j];
                match.Duration = eachMatchTime;
                match.GameScheduled = currentDateTime;
            }

            currentDateTime = currentDateTime.Add(eachMatchTime);
            matchesScheduledToday += parallelMatches;

            if (matchesScheduledToday >= matchPerDay * parallelMatches || currentDateTime.Hour >= endTime.Hour)
            {
                currentDateTime = GetModifiedStartDate(startTime, currentDateTime.AddDays(1));
                matchesScheduledToday = 0;
            }
        }

        return matches;
    }
}
