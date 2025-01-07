using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class Archive
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Year { get; set; }
        public DateOnly? LastDate { get; set; }
        public long TotalSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan Achievement
        {
            get => TimeSpan.FromSeconds(TotalSeconds);
            set => TotalSeconds = (long)value.TotalSeconds;
        }
        public int CompletedHabits { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public ICollection<HabitsArchive>? Habits { get; set; }
        public ICollection<ActivitiesArchive>? Activities { get; set; }
    }
}
