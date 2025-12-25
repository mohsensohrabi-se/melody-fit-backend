using MelodyFit.Domain.Users.Aggregates;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
