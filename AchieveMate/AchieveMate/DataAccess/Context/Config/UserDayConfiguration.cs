using AchieveMate.Models;
using AchieveMate.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AchieveMate.DataAccess.Context.Config
{
    public class UserDayConfiguration : IEntityTypeConfiguration<UserDay>
    {
        public void Configure(EntityTypeBuilder<UserDay> builder)
        {
            builder.HasKey(ud => ud.Id);

            builder.HasIndex(ud => ud.UserId)
                .IsUnique(false);

            //builder.Property(ud => ud.Achievement)
            //    .HasDefaultValue(new TimeSpan(0, 0, 0));

            builder.Property(ud => ud.Date)
                .HasColumnType("Date")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("getdate()");

            builder.Property(ud => ud.Evaluation)
                .HasConversion(
                e => e.ToString(),
                e => (DayEvaluation)Enum.Parse(typeof(DayEvaluation), e)
                );

            builder.Property(ud => ud.Evaluation)
                .HasDefaultValue(DayEvaluation.None);

            builder.ToTable("UsersDays");
        }
    }
}
