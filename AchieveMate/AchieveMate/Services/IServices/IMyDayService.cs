using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;

namespace AchieveMate.Services.IServices
{
    public interface IMyDayService
    {
        public Task<MyDayVM> GetMyDayAsync(int userId);
        public Task<bool> StartSessionAsync(int userId, StartSessionVM sessionVM);
        public Session? UpdateSessionStatus(int userId);
        public Task<TimeSpan?> FinishSessionAsync(int userId);
        public Task<bool> CancelSession(int userId);
        public Task<bool> UpdateMyHabitStatusAsync(int dayId, int habitId, bool isCompleted);
        public Task<bool> UpdateMyDayAsync(int userId, UpdateDayVM updateDayVM);
        public bool UpdateSessionNotes(int userId, string notes);
        public IQueryable<Activity> InProgressActivites(int userId);
    }
}
