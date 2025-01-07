namespace AchieveMate.Models
{
    public class DayHabits
    {
        public int DayId { get; set; }
        public int HabitId { get; set; }
        public bool IsCompleted { get; set; } = false;
        public UserDay Day { get; set; } = null!;
        public Habit Habit { get; set; } = null!;
    }
}
