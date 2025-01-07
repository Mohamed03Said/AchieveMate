

namespace AchieveMate.Helper
{
    public static class ExtensionMethods
    {
        public static string Display(this TimeSpan timeSpan)
        {
            int TotalHours = (int)timeSpan.TotalHours;
            return $"{TotalHours} h, {timeSpan.Minutes} m";
        }
    }
}
