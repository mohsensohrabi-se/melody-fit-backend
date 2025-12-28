

using System.ComponentModel.DataAnnotations.Schema;

namespace MelodyFit.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get;  protected set; } = Guid.NewGuid();
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = new();
    [NotMapped]
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
   
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent); 
    }
    public void ClearDomainEvent()
    {
        _domainEvents.Clear(); 
    }
}
