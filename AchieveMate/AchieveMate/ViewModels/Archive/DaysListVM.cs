using AchieveMate.Models.Enum;

namespace AchieveMate.ViewModels.Archive
{
    public class DaysListVM
    {
        public DateOnly Date { get; set; }
        public DayEvaluation Evaluation { get; set; }
        public TimeSpan Achievement { get; set; }
    }
}
