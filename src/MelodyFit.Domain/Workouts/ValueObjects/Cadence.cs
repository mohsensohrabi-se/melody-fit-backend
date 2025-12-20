using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Workouts.ValueObjects
{
    public sealed class Cadence : ValueObject
    {
        public int StepsPerMinute { get; }

        private const int Min = 10;
        private const int Max = 300;

        private Cadence(int spm)
        {
            StepsPerMinute = spm;
        }
        public static Result<Cadence> Create(int spm)
        {
            if (spm < Min || spm > Max)
                return Result.Failure<Cadence>($"Spm must be between {Min}-{Max}");
            return Result.Success<Cadence>(new Cadence(spm));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return StepsPerMinute;
        }
    }
}
