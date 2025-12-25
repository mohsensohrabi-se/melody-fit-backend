using MelodyFit.Domain.Users.Aggregates;
using MelodyFit.Domain.Users.Entities;
using MelodyFit.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MelodyFit.Infrastructure.Persistence
{
    public class MelodyFitDbContext : DbContext
    {
        public MelodyFitDbContext(DbContextOptions<MelodyFitDbContext> options) : base(options)
        {
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

    }
}
