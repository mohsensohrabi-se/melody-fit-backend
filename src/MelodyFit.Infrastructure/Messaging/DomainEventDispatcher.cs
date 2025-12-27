using MediatR;
using MelodyFit.Application.Common.Interfaces.Messaging;
using MelodyFit.Domain.Common;


namespace MelodyFit.Infrastructure.Messaging
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IEnumerable<DomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents) 
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
