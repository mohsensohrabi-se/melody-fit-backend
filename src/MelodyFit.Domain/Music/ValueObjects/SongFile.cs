using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Music.ValueObjects
{
    public sealed class SongFile : ValueObject
    {
        public string FileUrl { get; }
        public string FileType { get; }
        public long FileSize { get; }

        private static readonly string[] AllowedTypes = {".mp3",".wav"};
        private const long MaxSize = 20_000_000; //20M

        private SongFile(
            string fileUrl,
            string fileType,
            long fileSize) 
        {
            FileUrl = fileUrl; 
            FileType = fileType;
            FileSize = fileSize;
        }
        public static Result<SongFile> Create(string fileUrl, long fileSize)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return Result.Failure<SongFile>("File url is required.");

            if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out _))
                return Result.Failure<SongFile>("Invalid file URL");

            var extension = Path.GetExtension(fileUrl).ToLowerInvariant();
            if (!AllowedTypes.Contains(extension))
                return Result.Failure<SongFile>("Only mp3 and wav files are required");

            if (fileSize > MaxSize)
                return Result.Failure<SongFile>("File size must be lower than 20 MB.");
            
            var songFile = new SongFile(fileUrl, extension, fileSize);
            return Result.Success<SongFile>(songFile);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return FileUrl;
            yield return FileType;
            yield return FileSize;
        }
    }
}
