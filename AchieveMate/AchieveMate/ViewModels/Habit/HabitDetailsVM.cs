using AchieveMate.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Habit
{
    public class HabitDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public HabitType Type { get; set; }

        [Display(Name="Start Date")]
        public DateOnly StartDate { get; set; }

        [Display(Name = "Current Streak Date")]
        public DateOnly? StreakDate { get; set; }

        [Display(Name = "Current Streak")]
        public int Streak { get; set; }

        [Display(Name = "Completed")]
        public int Completed { get; set; }

        [Display(Name = "Max Streak")]
        public int MaxStreak { get; set; }
    }
}
