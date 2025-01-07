using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;

namespace AchieveMate.DataAccess.Repositories.IRepositories
{
    public interface IUserDayRepository
    {
        public Task<bool> AddSessionAsync(Session session);
        public Task<UserDay> GetOrAddUserDayAsync(int userId);
        public Task<UserDay> GetUserDayByIdAsync(int dayId);
        public Task<bool> UpdateUserDayAsync(UserDay editedUserDay);
        public Task SaveChangesAsync();
        public Task<List<DayHabits>> GetUserDayHabits(int userId, int dayId);
        public Task<bool> AddDayHabitAsync(DayHabits dayHabit);
        public Task<DayHabits?> GetDayHabitAsync(int dayId, int habitId);
        public Task<bool> IsDayHabitExistAsync(int dayId, int habitId);
        public Task<bool> UpdateDayHabitStatusAsync(DayHabits dayHabit);
        public Task<bool> UpdateSessionAsync(Session session);
        public Task<bool> RemoveSessionAsync(Session session);
    }
}
