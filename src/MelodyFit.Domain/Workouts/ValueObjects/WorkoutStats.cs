using MelodyFit.Domain.Common;
using MelodyFit.Domain.Workouts.Entities;


namespace MelodyFit.Domain.Workouts.ValueObjects
{
    public sealed class WorkoutStats : ValueObject
    {
        // Cadence
        public Cadence? AverageCadence { get; }
        public Cadence? MaxCadence { get; }
        public Cadence? MinCadence { get; }
        
        // Pace 
        public Pace? BestPace { get; }
        public Pace? WorstPace { get; }

        // 
        public double DistanceKm { get; }
        public double AvergaeSpeedKmPerHour { get; }
        public WorkoutDuration Duration { get; }
        public int TotalSteps { get; }

        private WorkoutStats(
            Cadence? averageCadence,
            Cadence? maxCadence,
            Cadence? minCadence,
            Pace? bestPace,
            Pace? worstPacce,
            double distanceKm,
            double averageSpeedPerHour,
            WorkoutDuration duration,
            int totalSteps

            )
        {
            AverageCadence = averageCadence;
            MaxCadence = maxCadence;
            MinCadence = minCadence;
            BestPace = bestPace;
            WorstPace = worstPacce;
            DistanceKm = distanceKm;
            AvergaeSpeedKmPerHour = averageSpeedPerHour;
            Duration = duration;
            TotalSteps = totalSteps;
        }

        public static Result<WorkoutStats> Create(
            IEnumerable<WorkoutDataPoint> dataPoints,
            DateTime startTime,
            DateTime endTime,
            double distanceMeters
            )
        {
            if (!dataPoints.Any())
                return Result.Failure<WorkoutStats>("There is no data points");
            var duration = WorkoutDuration.From( endTime - startTime ).Value;

            var cadenceValues = dataPoints
                .Where(p=>p.Cadence !=null)
                .Select(p=>p.Cadence!.StepsPerMinute)
                .ToList();

            var paceValues = dataPoints
                .Where (p=>p.Pace != null)
                .Select(p=>p.Pace!)
                .ToList();

            var totalStesp = dataPoints.Sum(p => p.Steps);
            var distanceKm = distanceMeters / 1000;
            var avgSpeed = duration.Value.TotalHours > 0 ? distanceKm / duration.Value.TotalHours : 0;

            var workoutStats = new WorkoutStats(
                cadenceValues.Any()? Cadence.Create((int) cadenceValues.Average()).Value:null,
                cadenceValues.Any()? Cadence.Create(cadenceValues.Max()).Value:null,
                cadenceValues.Any()?Cadence.Create(cadenceValues.Min()).Value:null,
                paceValues.Any()? paceValues.Min() :null,
                paceValues.Any()? paceValues.Max():null,
                distanceKm,
                avgSpeed,
                duration,
                totalStesp
                );
            return Result.Success<WorkoutStats>(workoutStats);
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return AverageCadence;
            yield return MaxCadence;
            yield return MinCadence;
            yield return BestPace;
            yield return WorstPace;
            yield return DistanceKm;
            yield return Duration;
            yield return AvergaeSpeedKmPerHour;
            yield return TotalSteps;
        }
    }
}
