using MelodyFit.Domain.Common;
using MelodyFit.Domain.Workouts.ValueObjects;


namespace MelodyFit.Domain.Workouts.Entities
{
    public class WorkoutDataPoint : BaseEntity
    {
        public DateTime TimeStamp { get; }
        public Cadence? Cadence { get; }
        public Pace? Pace { get; }
        public GeoPoint? Location { get; }
        public int Steps { get; }

        private WorkoutDataPoint(
            DateTime timeStamp,
            Cadence? cadence,
            Pace? pace,
            GeoPoint? location,
            int steps
            ) 
        {
            TimeStamp = timeStamp;
            Cadence = cadence;
            Pace = pace;
            Location = location;
            Steps = steps;
        }  

        public static Result<WorkoutDataPoint> Create(
            DateTime timeStamp,
            Cadence? cadence,
            Pace? pace,
            GeoPoint location,
            int steps
            )
        {
            if (timeStamp == default)
                return Result.Failure<WorkoutDataPoint>("Timstamp is required");
            if (steps < 0)
                return Result.Failure<WorkoutDataPoint>("Steps must be a positive number");

            var workoutDataPoint = new WorkoutDataPoint(timeStamp, cadence, pace, location, steps);
            return Result.Success<WorkoutDataPoint>(workoutDataPoint);
        }



    }
}
