

namespace MelodyFit.Domain.Common;

public abstract  record DomainEvent(Guid EventId, DateTime OccuredOn)
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }
}
