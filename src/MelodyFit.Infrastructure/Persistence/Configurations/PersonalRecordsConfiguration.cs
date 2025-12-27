using MelodyFit.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MelodyFit.Infrastructure.Persistence.Configurations
{
    public class PersonalRecordsConfiguration : IEntityTypeConfiguration<PersonalRecords>
    {
        public void Configure(EntityTypeBuilder<PersonalRecords> builder)
        {
            builder.ToTable("PersonalRecords");

            builder.HasKey(pr => pr.Id);

            builder.Property<Guid>("UserId")
                .IsRequired();

            builder.Property(pr => pr.MaxBurnedCalories)
                .IsRequired();

            builder.Property(pr=>pr.MaxSteps)
                .IsRequired();

            builder.Property(pr => pr.LongestDistanceKm)
                .IsRequired();

            builder.OwnsOne(pr => pr.MaxWorkoutDuration, duration => 
            {
                duration.Property(wd=>wd.Value)
                    .HasColumnName("MaxWorkoutDurationSeconds")
                    .IsRequired();
            });

            builder.HasIndex("UserId")
                .IsUnique();

        }
    }
}
