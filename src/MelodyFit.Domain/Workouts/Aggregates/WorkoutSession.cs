using MelodyFit.Domain.Common;
using MelodyFit.Domain.Workouts.Entities;
using MelodyFit.Domain.Workouts.ValueObjects;



namespace MelodyFit.Domain.Workouts.Aggregates
{
    public sealed class WorkoutSession : AggregateRoot
    {
        public Guid UserId { get; }
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        private readonly List<WorkoutDataPoint> _datapoints = new();
        public IReadOnlyCollection<WorkoutDataPoint>  DataPoints  => _datapoints.AsReadOnly();

        private WorkoutSession(Guid userId, DateTime startTime)
        {
            UserId = userId;
            StartTime = startTime;
        }

        public static Result<WorkoutSession> Start(Guid userId)
        {
            if (userId == Guid.Empty)
                return Result.Failure<WorkoutSession>("User Id is required");
            return Result.Success<WorkoutSession>(new WorkoutSession(userId,DateTime.UtcNow));
        }

        public Result End(double weight)
        {
            if (EndTime is not null)
                return Result.Failure("Workout already ended");
            EndTime = DateTime.UtcNow;

            var distance = EstimateDistanceMeters();
            var calories = EstimateCalories(weight);
            AddDomainEvent(new WorkoutCompletedEvent(Id,UserId,distance,calories));

            return Result.Success();
        }

        public Result AddDataPoint(WorkoutDataPoint datapoint)
        {
            if (EndTime is not null)
                return Result.Failure("You can add data point for a finished workout");
            _datapoints.Add(datapoint);
            return Result.Success();
        }

        public Cadence? CalculateAverageCadence()
        {
            var cadenceValues = _datapoints
                                    .Where(p => p.Cadence != null)
                                    .Select(p => p.Cadence!.StepsPerMinute)
                                    .ToList();
            if(!cadenceValues.Any())
                return null;
            return Cadence.Create((int) cadenceValues.Average()).Value;
        }

        public double EstimateDistanceMeters()
        {
            var points = _datapoints
                .Where(p => p.Location != null)
                .OrderBy(p => p.TimeStamp)
                .Select(p => p.Location!)
                .ToList();

            if (points.Count < 2)
                return 0;

            const double EarthRadius = 6_371_000; // meters
            double distance = 0;

            for (int i = 1; i < points.Count; i++)
            {
                var prev = points[i - 1];
                var curr = points[i];

                var lat1 = DegreesToRadians(prev.Latitude);
                var lon1 = DegreesToRadians(prev.Longitude);
                var lat2 = DegreesToRadians(curr.Latitude);
                var lon2 = DegreesToRadians(curr.Longitude);

                var dLat = lat2 - lat1;
                var dLon = lon2 - lon1;

                var a =
                    Math.Pow(Math.Sin(dLat / 2), 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Pow(Math.Sin(dLon / 2), 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                distance += EarthRadius * c;
            }

            return distance;
        }

        public int EstimateCalories(double weight)
        {
            // calories ~ MET * weight * Duration
            if (!_datapoints.Any()) return 0;

            var avgCadence = CalculateAverageCadence()?.StepsPerMinute ?? 0;
            var durationHours = ((EndTime ?? DateTime.UtcNow) - StartTime).TotalHours;

            //Rough MET estimation
            double met = avgCadence switch
            {
                < 90 => 3.0,
                < 120 => 6.0,
                < 150 => 8.0,
                _ => 10.0
            };

            return (int)(met * weight * durationHours);

        }

        public int CalculateTempoScore(int songBpm)
        {
            var cadenceValues = _datapoints
                .Where(p => p.Cadence != null)
                .Select(p => p.Cadence!.StepsPerMinute)
                .ToList();

            if (!cadenceValues.Any())
                return 0;

            var avgCadence = cadenceValues.Average();

            // Ideal cadence-to-BPM alignment
            // Walking ≈ 1 step ≈ 1 beat
            var diff = Math.Abs(avgCadence - songBpm);

            var score = Math.Max(0, 100 - diff);
            return (int)score;
        }

        public WorkoutSummary CalculateSummary(
            double weightKg,
            int songBpm)
        {
            return WorkoutSummary.Create(
                distanceKm: EstimateDistanceMeters()/1000.0,
                duration: WorkoutDuration.From(
                    (EndTime ?? DateTime.UtcNow) - StartTime
                ).Value,
                steps: _datapoints.Sum(p => p.Steps),
                calories: EstimateCalories(weightKg),
                bpmAlignmentScore: CalculateTempoScore(songBpm),
                date: DateOnly.FromDateTime(StartTime)
            ).Value;
        }

        public Result<WorkoutStats> GetStats()
        {
            if (!_datapoints.Any())
                return Result.Failure<WorkoutStats>("No workout data available");

            var end = EndTime ?? DateTime.UtcNow;

            return WorkoutStats.Create(
                dataPoints: _datapoints,
                startTime: StartTime,
                endTime: end,
                distanceMeters: EstimateDistanceMeters()
            );
        }

        private static double DegreesToRadians(double deg) => deg * Math.PI / 180;
    }
}
