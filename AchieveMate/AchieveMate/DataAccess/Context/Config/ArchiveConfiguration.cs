using AchieveMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class ArchiveConfiguration : IEntityTypeConfiguration<Archive>
    {
        public void Configure(EntityTypeBuilder<Archive> builder)
        {
            builder.HasKey(a => a.Id);

            //builder.Property(a => a.SpentTime)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            builder.HasMany(a => a.Habits)
                .WithOne(h => h.Archive)
                .HasForeignKey(h => h.ArchiveId)
                .IsRequired(true);

            builder.HasMany(a => a.Activities)
                .WithOne(g => g.Archive)
                .HasForeignKey(h => h.ArchiveId)
                .IsRequired(true);

            builder.Property(a => a.LastDate)
                .HasColumnType("date");

            builder.ToTable("Archives");
        }
    }
}
