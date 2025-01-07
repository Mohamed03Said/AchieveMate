using AchieveMate.Attributes;
using AchieveMate.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Activity
{
    public class ActivityVM
    {
        public int? Id { get; set; }

        [Required]
        [Remote(action: "ActivityValidation", controller: "Activity", 
            AdditionalFields = nameof(Id), ErrorMessage = "Activity name is already exists.")]
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }

        [Display(Name = "Elapsed Time")]
        public TimeSpan SpentTime { get; set; }
        public ActivityStatus Status { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartAt { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndAt { get; set; }

        [Display(Name = "Expiry Date")]
        public DateOnly? ExpiryDate { get; set; }
    }
}
