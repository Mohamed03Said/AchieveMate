using AchieveMate.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class UserDay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public long TotalSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan Achievement
        {
            get => TimeSpan.FromSeconds(TotalSeconds);
            set => TotalSeconds = (long)value.TotalSeconds;
        }
        public DayEvaluation Evaluation { get; set; } = DayEvaluation.None;
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public string? Notes { get; set; }
        public ICollection<Session>? Sessions { get; set; }
        public ICollection<DayHabits>? Habits { get; set; }
    }
}
