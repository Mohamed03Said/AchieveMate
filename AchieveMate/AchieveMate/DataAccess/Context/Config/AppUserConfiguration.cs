using AchieveMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email)
                .IsUnique(true);

            builder.Property(u => u.UserName)
                .IsRequired(false);

            builder.Property(u => u.Email)
                .IsRequired(true);

            builder.HasMany(u => u.Archives)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .IsRequired(true);

            builder.HasMany(u => u.Days)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .IsRequired(true);

            builder.HasMany(u => u.Habits)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .IsRequired(true);

            builder.HasMany(u => u.Activities)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .IsRequired(true);

            builder.ToTable("AppUsers");
        }
    }
}
