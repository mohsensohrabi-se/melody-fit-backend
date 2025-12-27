using MelodyFit.Domain.Common;


namespace MelodyFit.Application.Common.Interfaces.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<DomainEvent> domainEvents);
    }
}
