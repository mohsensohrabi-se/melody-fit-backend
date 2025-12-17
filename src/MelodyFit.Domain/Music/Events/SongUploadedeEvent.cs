using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Music.Events
{
    public sealed record SongUploadedEvent(Guid SongId, Guid UserId):DomainEvent;

}
