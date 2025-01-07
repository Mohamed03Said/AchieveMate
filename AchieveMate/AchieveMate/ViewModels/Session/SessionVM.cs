using AchieveMate.Models.Enum;

namespace AchieveMate.ViewModels.Session
{
    public class SessionVM
    {
        public string Activity { get; set; }
        public SessionType Type { get; set; }
        public SessionStatus Status { get; set; }
        public TimeSpan Time { get; set; }
        public string Notes { get; set; }
    }
}
