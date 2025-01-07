using AchieveMate.Models;
using AchieveMate.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);

            //builder.Property(s => s.ElapsedTime)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            //builder.Property(s => s.PauseTime)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            builder.HasOne(s => s.Activity)
                .WithMany(g => g.Sessions)
                .HasForeignKey(s => s.ActivityId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Day)
                .WithMany(d => d.Sessions)
                .HasForeignKey(s => s.DayId)
                .IsRequired(true);

            builder.Property(s => s.Type)
                .HasConversion(
                t => t.ToString(),
                t => (SessionType)Enum.Parse(typeof(SessionType), t)
                );

            builder.Property(s => s.Status)
                .HasConversion(
                t => t.ToString(),
                t => (SessionStatus)Enum.Parse(typeof(SessionStatus), t)
                );

            builder.Property(s => s.Status)
                .HasDefaultValue(SessionStatus.InProgress);

            builder.ToTable("Sessions");
        }
    }
}
