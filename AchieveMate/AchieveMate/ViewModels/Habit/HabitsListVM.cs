using AchieveMate.Models.Enum;

namespace AchieveMate.ViewModels.Habit
{
    public class HabitsListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public HabitType Type { get; set; }
        public int Streak { get; set; }
    }
}
