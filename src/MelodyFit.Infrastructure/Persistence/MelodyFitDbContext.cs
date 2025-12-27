using MelodyFit.Application.Common.Interfaces.Messaging;
using MelodyFit.Domain.Common;
using MelodyFit.Domain.Users.Aggregates;
using MelodyFit.Domain.Users.Entities;
using MelodyFit.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MelodyFit.Infrastructure.Persistence
{
    public class MelodyFitDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        public MelodyFitDbContext(
            DbContextOptions<MelodyFitDbContext> options,
            IDomainEventDispatcher domainEventDispatcher) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<PersonalRecords> PersonalRecords => Set<PersonalRecords>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
                );

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entitiesWithEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entitiesWithEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            await _domainEventDispatcher.DispatchAsync(domainEvents);
            foreach(var entity in ChangeTracker.Entries<BaseEntity>())
            {
                entity.Entity.ClearDomainEvent();
            }
            return result;
        }
             

    }
}
