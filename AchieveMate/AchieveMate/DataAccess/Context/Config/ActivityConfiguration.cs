using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AchieveMate.DataAccess.Context.Config
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(g => g.Id);

            builder.HasIndex(g => g.Name)
                .IsUnique(false);

            //builder.Property(g => g.SpentTime)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            builder.Property(g => g.ExpiryDate)
                .HasColumnType("Date");

            builder.ToTable("Activities");
        }
    }
}