using MelodyFit.Domain.Common;

public sealed record WorkoutCompletedEvent(
    Guid SessionId,
    Guid UserId,
    double DistanceMeters,
    double Calories
) : DomainEvent;
