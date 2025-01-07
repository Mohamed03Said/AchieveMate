using AchieveMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class HabitsArchiveConfiguration : IEntityTypeConfiguration<HabitsArchive>
    {
        public void Configure(EntityTypeBuilder<HabitsArchive> builder)
        {
            builder.HasKey(ha => new { ha.ArchiveId, ha.Name });

            builder.ToTable("HabitsArchives");
        }
    }
}
