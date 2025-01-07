using AchieveMate.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Habit
{
    public class HabitVM
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "HabitValidation", controller: "Habit",
            AdditionalFields = nameof(Id), ErrorMessage = "Habit name is already exists.")]
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public HabitType Type { get; set; }
    }
}
