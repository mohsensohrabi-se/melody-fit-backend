using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get;  protected set; } = Guid.NewGuid();
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent); 
    }
    protected void ClearDomainEvent()
    {
        _domainEvents.Clear(); 
    }
}
