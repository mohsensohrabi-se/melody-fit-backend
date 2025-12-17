using MelodyFit.Domain.Common;
using MelodyFit.Domain.Music.Enums;


namespace MelodyFit.Domain.Music.ValueObjects
{
    public sealed class BpmCategory : ValueObject
    {
        public BpmCategoryEnum Type { get; }

        private BpmCategory(BpmCategoryEnum type)
        {
            Type = type;
        }

        public static BpmCategory FromBpm(int bpm)
        {
            return bpm switch
            {
                <= 100 => new BpmCategory(BpmCategoryEnum.Relaxed),
                <= 125 => new BpmCategory(BpmCategoryEnum.Moderate),
                <=150 => new BpmCategory(BpmCategoryEnum.Fast),
                _ => new BpmCategory(BpmCategoryEnum.Intense)

            };
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
        }
    }
}
