using AchieveMate.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchieveMate.Models
{
    public class Habit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public HabitType Type { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? LastCompletedDate { get; set; }
        public DateOnly? StreakDate { get; set; }
        public int Streak { get; set; } = 0;
        public int Completed { get; set; } = 0;
        public int MaxStreak { get; set; } = 0;
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public ICollection<DayHabits>? Days { get; set; }
    }
}
