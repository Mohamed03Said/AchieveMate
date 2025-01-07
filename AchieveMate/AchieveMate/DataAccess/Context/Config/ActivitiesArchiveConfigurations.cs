using AchieveMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class ActivitiesArchiveConfigurations : IEntityTypeConfiguration<ActivitiesArchive>
    {
        public void Configure(EntityTypeBuilder<ActivitiesArchive> builder)
        {
            builder.HasKey(ga => new { ga.ArchiveId, ga.Name });

            //builder.Property(ga => ga.SpentTime)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            builder.ToTable("ActivitiesArchives");
        }
    }
}
