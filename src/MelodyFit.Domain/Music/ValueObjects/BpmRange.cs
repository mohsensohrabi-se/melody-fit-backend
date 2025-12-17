using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Music.ValueObjects
{
    public sealed class BpmRange : ValueObject
    {
        public int Min { get; }
        public int Max { get; }

        private const int MinBpm = 30;
        private const int MaxBpm = 250;
        private BpmRange(int min, int max)
        {
            Min = min; 
            Max = max; 
        }

        public static Result<BpmRange> Create(int min, int max)
        {
            if (min < MinBpm || max > MaxBpm)
                return Result.Failure<BpmRange>($"Bpm must be between {MinBpm}-{MaxBpm}");
            if (min > max)
                return Result.Failure<BpmRange>("Min range must be lower than max");

            var range = new BpmRange(min, max);
            return Result.Success<BpmRange>(range);
        }

        public bool Contains(int bpm) => bpm>=Min && bpm<=Max;
        public int Distance(int bpm)
        {
            if (Contains(bpm)) return 0;
            if(bpm < Min) 
                return Min - bpm;
            return bpm - Max;
        } 
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Min;
            yield return Max;
        }
    }
}
