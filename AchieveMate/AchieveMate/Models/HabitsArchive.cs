namespace AchieveMate.Models
{
    public class HabitsArchive
    {//Weak Entity
        public int ArchiveId { get; set; }
        public string Name { get; set; } = null!;
        public int Target { get; set; }
        public int Achievement { get; set; }
        public bool IsActive { get; set; }
        public Archive Archive { get; set; } = null!;
    }
}
