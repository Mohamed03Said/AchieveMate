using AchieveMate.ViewModels.Activity;
using AchieveMate.ViewModels.Habit;

namespace AchieveMate.Services.IServices
{
    public interface IHabitService
    {
        public Task<List<HabitsListVM>> GetMyHabitsAsync(int userId);
        public Task<HabitDetailsVM?> GetHabitAsync(int userId, int habitId);
        public Task<bool?> UpdateHabitAsync(HabitVM habitVM, int habitId, int userId);
        public Task<bool> AddHabitAsync(HabitVM habitVM, int userId);
        public Task<bool> IsUniqueHabitName(int userId, int habitId, string habitName);
    }
}
