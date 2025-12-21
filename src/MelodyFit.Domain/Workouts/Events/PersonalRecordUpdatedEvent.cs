using MelodyFit.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Domain.Workouts.Events
{
    public sealed record PersonalRecordUpdatedEvent(Guid UserId, DateOnly Date):DomainEvent;
    
}
