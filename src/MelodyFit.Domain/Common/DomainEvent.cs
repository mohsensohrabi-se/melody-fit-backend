

using MediatR;

namespace MelodyFit.Domain.Common;

public abstract  record DomainEvent(Guid EventId, DateTime OccuredOn) : INotification
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }
}
