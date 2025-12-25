using MelodyFit.Domain.Users.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MelodyFit.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);

            builder.Property(u => u.CreatedAt)
                .IsRequired();
            
            builder.Property(u=>u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.OwnsOne(u => u.Email, email => 
            {
                email.Property(e => e.Value)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(256);
                email.HasIndex(e=>e.Value).IsUnique();

            });

            builder.OwnsOne(u => u.Profile, profile => 
            {
                profile.Property(p=>p.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                
                profile.Property(p=>p.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                profile.Property(p => p.Gender).HasMaxLength(20);
                profile.Property(p => p.BirthDate);
                profile.Property(p => p.WeightKg);
                profile.Property(p => p.HeightCm);
            });
        }
    }
}
