using MelodyFit.Domain.Common;

namespace MelodyFit.Domain.Users.Events
{
    public sealed record UserRegisteredEvent(Guid UserId):DomainEvent;
    
}
