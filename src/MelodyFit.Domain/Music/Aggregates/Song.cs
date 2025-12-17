using MelodyFit.Domain.Common;
using MelodyFit.Domain.Music.Events;
using MelodyFit.Domain.Music.ValueObjects;
using MelodyFit.Domain.Users.Aggregates;


namespace MelodyFit.Domain.Music.Aggregates
{
    public sealed class Song:AggregateRoot
    {
        private Song() { }
        public Guid UserId { get; private set; }
        public string Title { get; private set; } = default!;
        public string Artist { get; private set; } = default!;
        public int Duration { get; private set; }
        public int Bpm { get; private set; }
        public BpmRange BpmRange { get; private set; } = default!;
        public BpmCategory TempoCategory { get; private set; } = default!;
        public SongFile File { get; private set; } = default!;
        public DateTime UploadedAt { get; private set; }


        public static Result<Song> Upload(
            Guid userId,
            string title,
            string artist,
            int duration, 
            int bpm,
            SongFile file
            )
        {
            if (userId == Guid.Empty)
                return Result.Failure<Song>("Invalid user Id");
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Song>("Title is required");
            if (string.IsNullOrWhiteSpace(artist))
                return Result.Failure<Song>("Artist is required");
            if (duration <= 0)
                return Result.Failure<Song>("Duration must be a positive number");
            if (bpm < 30 || bpm >250)
                return Result.Failure<Song>($"Bpm must be between 30 - 250 ");
            if (file is null)
                return Result.Failure<Song>("File is required");
            var bpmResult = BpmRange.Create(Math.Max(bpm - 5, 0), bpm + 5);
            if (bpmResult.IsFailure)
                return Result.Failure<Song>(bpmResult.Error);
            var song = new Song
            {
                UserId = userId,
                Title = title,
                Artist = artist,
                Duration = duration,
                Bpm = bpm,
                BpmRange = bpmResult.Value,
                TempoCategory = BpmCategory.FromBpm(bpm),
                File = file,
                UploadedAt = DateTime.UtcNow
            };

            song.AddDomainEvent(new SongUploadedEvent(song.Id,userId));
            return Result.Success(song);
        }
    }
}
