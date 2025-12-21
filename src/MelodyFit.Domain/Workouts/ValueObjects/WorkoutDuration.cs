using MelodyFit.Domain.Common;

public sealed class WorkoutDuration : ValueObject
{
    public TimeSpan Value { get; }

    private static readonly TimeSpan Min = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan Max = TimeSpan.FromHours(10);

    private WorkoutDuration(TimeSpan value)
    {
        Value = value;
    }

    public static Result<WorkoutDuration> From(TimeSpan value)
    {
        if (value < TimeSpan.Zero)
            return Result.Failure<WorkoutDuration>("Duration cannot be negative");

        if (value < Min || value > Max)
            return Result.Failure<WorkoutDuration>(
                $"Workout duration must be between {Min} and {Max}");

        return Result.Success(new WorkoutDuration(value));
    }
    public static WorkoutDuration Zero => new(TimeSpan.Zero);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
