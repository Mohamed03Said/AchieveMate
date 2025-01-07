using AchieveMate.Models.Enum;

namespace AchieveMate.ViewModels.Habit
{
    public class DayHabitsListVM
    {
        public string Habit {  get; set; }
        public bool IsCompleted { get; set; }
        public HabitType Type { get; set; }
    }
}
