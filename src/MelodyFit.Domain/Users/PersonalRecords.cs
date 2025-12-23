using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Users
{
    public class PersonalRecords : BaseEntity
    {
        public WorkoutDuration MaxWorkoutDuration { get; private set; }
        public int MaxBurnedCalories { get; private set; }
        public int MaxSteps { get; private set; }
        public double LongestDistanceKm { get; private set; }

        private PersonalRecords()
        {
            MaxWorkoutDuration = WorkoutDuration.Zero;
            MaxBurnedCalories = 0;
            MaxSteps = 0;
            LongestDistanceKm = 0;
        }

        public static PersonalRecords Create()
            => new PersonalRecords();

        public bool TryUpdate(WorkoutSummary summary)
        {
            bool isNewRecord = false;

            if (summary.Duration.Value > MaxWorkoutDuration.Value)
            {
                MaxWorkoutDuration = summary.Duration;
                isNewRecord = true;
            }

            if (summary.Calories > MaxBurnedCalories)
            {
                MaxBurnedCalories = summary.Calories;
                isNewRecord = true;
            }

            if (summary.Steps > MaxSteps)
            {
                MaxSteps = summary.Steps;
                isNewRecord = true;
            }

            if (summary.DistanceKm > LongestDistanceKm)
            {
                LongestDistanceKm = summary.DistanceKm;
                isNewRecord = true;
            }

            return isNewRecord;
        }
    }
}
