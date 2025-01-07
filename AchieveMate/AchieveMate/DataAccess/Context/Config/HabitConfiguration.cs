using AchieveMate.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.HasKey(h => h.Id);

            builder.HasIndex(h => h.Name)
                .IsUnique(false);

            builder.Property(h => h.Type)
                .HasConversion(
                t => t.ToString(),
                t => (HabitType)Enum.Parse(typeof(HabitType), t)
                );

            builder.Property(h => h.StreakDate)
                .HasColumnType("Date");

            builder.Property(h => h.LastCompletedDate)
                .HasColumnType("Date");

            builder.Property(h => h.StartDate)
                .HasColumnType("Date")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("getdate()");

            builder.ToTable("Habits");
        }
    }
}