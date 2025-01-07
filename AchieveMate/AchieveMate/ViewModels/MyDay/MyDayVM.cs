using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.Session;

namespace AchieveMate.ViewModels.MyDay
{
    public class MyDayVM
    {
        public DateOnly Date { get; set; }
        public List<DayHabits>? Habits { get; set; }
        public SessionVM? Session { get; set; }
        public string? Notes { get; set; }
        public DayEvaluation Evaluation { get; set; }
        public TimeSpan Achievement { get; set; }
    }
}
