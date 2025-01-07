using AchieveMate.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public SessionType Type { get; set; }
        public SessionStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public long TotalSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan ElapsedTime
        {
            get => TimeSpan.FromSeconds(TotalSeconds);
            set => TotalSeconds = (long)value.TotalSeconds;
        }
        public long TotalPauseSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan PauseTime
        {
            get => TimeSpan.FromSeconds(TotalPauseSeconds);
            set => TotalPauseSeconds = (long)value.TotalSeconds;
        }
        public string? Notes { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
        public int DayId { get; set; }
        public UserDay Day { get; set; } = null!;
    }
}
