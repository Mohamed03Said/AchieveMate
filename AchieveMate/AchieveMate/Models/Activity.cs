using AchieveMate.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public long TotalSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan SpentTime
        {
            get => TimeSpan.FromSeconds(TotalSeconds);
            set => TotalSeconds = (long)value.TotalSeconds;
        }
        public ActivityStatus Status { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public AppUser User { get; set; } = null!;
        public int UserId { get; set; }
        public ICollection<Session>? Sessions { get; set; }
    }
}
