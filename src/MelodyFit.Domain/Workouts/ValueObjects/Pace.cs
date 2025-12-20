using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Workouts.ValueObjects
{
    public sealed class Pace : ValueObject
    {
        // It is calculate based on min/km
        // 3:00 min/km means it takes 3 min for a runner to cover a distance of 1km
        public TimeSpan Value { get; }

        private static readonly TimeSpan Min = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan Max = TimeSpan.FromMinutes(30);

        private Pace(TimeSpan value)
        {
            Value = value; 
        }

        public static Result<Pace> From(TimeSpan pace)
        {
            if(pace < Min || pace > Max)
               return Result.Failure<Pace>($"Pace must be between {Min} - {Max}");
            return Result.Success<Pace>(new Pace(pace));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
