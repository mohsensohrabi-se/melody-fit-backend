using MelodyFit.Domain.Common;

public sealed class WorkoutSummary : ValueObject
{
    public double DistanceKm { get; }
    public WorkoutDuration Duration { get; }
    public int Steps { get; }
    public int Calories { get; }
    public double BpmAlignmentScore { get; }
    public int MatchedSongsCount { get; }
    public DateOnly Date { get; }

    private WorkoutSummary(
        double distanceKm,
        WorkoutDuration duration,
        int steps,
        int calories,
        double bpmAlignmentScore,
        int matchedSongsCount,
        DateOnly date)
    {
        DistanceKm = distanceKm;
        Duration = duration;
        Steps = steps;
        Calories = calories;
        BpmAlignmentScore = bpmAlignmentScore;
        MatchedSongsCount = matchedSongsCount;
        Date = date;
    }

    public static Result<WorkoutSummary> Create(
        double distanceKm,
        WorkoutDuration duration,
        int steps,
        int calories,
        double bpmAlignmentScore,
        int matchedSongsCount,
        DateOnly date)
    {
        if (distanceKm <= 0)
            return Result.Failure<WorkoutSummary>("Distance must be positive");

        if (steps <= 0)
            return Result.Failure<WorkoutSummary>("Steps must be positive");

        if (calories < 0)
            return Result.Failure<WorkoutSummary>("Calories cannot be negative");

        if (bpmAlignmentScore < 0 || bpmAlignmentScore > 100)
            return Result.Failure<WorkoutSummary>("BPM alignment must be 0–100");

        return Result.Success(
            new WorkoutSummary(
                distanceKm,
                duration,
                steps,
                calories,
                bpmAlignmentScore,
                matchedSongsCount,
                date));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DistanceKm;
        yield return Duration;
        yield return Steps;
        yield return Calories;
        yield return BpmAlignmentScore;
        yield return MatchedSongsCount;
        yield return Date;
    }
}
