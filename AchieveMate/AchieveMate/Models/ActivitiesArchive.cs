using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class ActivitiesArchive
    {//Weak Entity
        public int ArchiveId { get; set; }
        public string Name { get; set; } = null!;
        public int Sessions { get; set; }
        public long TotalSeconds { get; set; } = 0;

        [NotMapped]
        public TimeSpan SpentTime
        {
            get => TimeSpan.FromSeconds(TotalSeconds);
            set => TotalSeconds = (long)value.TotalSeconds;
        }
        public bool IsFinished { get; set; }
        public Archive Archive { get; set; } = null!;
    }
}
