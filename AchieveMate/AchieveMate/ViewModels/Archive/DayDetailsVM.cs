using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.MyDay;

namespace AchieveMate.ViewModels.Archive
{
    public class DayDetailsVM
    {
        public List<SessionsListVM>? Sessions { get; set; }
        public List<DayHabitsListVM>? Habits { get; set; }
        public string? Notes { get; set; }
        public DayEvaluation Evaluation { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan Achievement { get; set; }
    }
}
