namespace AchieveMate.ViewModels.Archive
{
    public class ArchiveYearVM
    {
        public int Year { get; set; }
        public int CompletedHabits { get; set; }
        public TimeSpan AchieveMent { get; set; }
        public List<HabitArchiveVM>? Habits { get; set; }
        public List<ActivityArchiveVM>? Activities { get; set; }
    }

    public class HabitArchiveVM
    {
        public string Name { get; set; } = null!;
        public int Completed { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivityArchiveVM
    {
        public string Name { get; set; } = null!;
        public TimeSpan SpentTime { get; set; }
        public int Sessions { get; set; }
        public bool IsFinished { get; set; }
    }
}
