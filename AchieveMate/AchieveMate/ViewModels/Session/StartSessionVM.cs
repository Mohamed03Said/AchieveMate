using AchieveMate.Models.Enum;
using AchieveMate.Models;
using AchieveMate.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Session
{
    [RecordedSessionValidations]
    [TimerSessionValidations]
    public class StartSessionVM
    {
        [Range(0, 2, ErrorMessage = "Select a valid session type")]
        public SessionType Type { get; set; }
        [Required]
        public int Activity { get; set; }
        public string? Notes { get; set; }
        public List<Models.Activity>? Activities { get; set; }

        // Timer
        public TimeSpan? InitialTimer { get; set; }

        // Recorded
        public DateTime? StartAt { get; set; }
        public TimeSpan? ElapsedTime { get; set; }

        // Stopwatch
    }
}
