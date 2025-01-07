using AchieveMate.Models.Enum;

namespace AchieveMate.ViewModels.Activity
{
    public class ActivitiesListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TimeSpan SpentTime { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateOnly? ExpiryDate { get; set; }
    }
}
