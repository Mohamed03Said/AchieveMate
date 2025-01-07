using AchieveMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class DayHabitsConfiguration : IEntityTypeConfiguration<DayHabits>
    {
        public void Configure(EntityTypeBuilder<DayHabits> builder)
        {
            builder.HasKey(dh => new { dh.HabitId, dh.DayId });

            /*builder.HasMany(h => h.UserDays)
            .WithMany(d => d.Habits)
            .UsingEntity<DayHabits>(
            l => l.HasOne<Habit>().WithMany().HasForeignKey(dh => dh.HabitId)
            .OnDelete(DeleteBehavior.Restrict)
            ,
            r => r.HasOne<UserDay>().WithMany().HasForeignKey(dh => dh.DayId)
            .OnDelete(DeleteBehavior.Restrict)
            );*/

            builder.HasOne(dh => dh.Habit)
                .WithMany(h => h.Days)
                .HasForeignKey(dh => dh.HabitId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dh => dh.Day)
                .WithMany(h => h.Habits)
                .HasForeignKey(dh => dh.DayId)
                .IsRequired(true);

            builder.Property(dh => dh.IsCompleted)
                .HasDefaultValue(false);

            builder.ToTable("DaysHabits");
        }
    }
}