using AchieveMate.ViewModels.Habit;

namespace AchieveMate.DataAccess.Repositories.IRepositories
{
    public interface IHabitRepository
    {
        public IQueryable<Habit> GetAllUserHabits(int userId);
        public IQueryable<Habit> GetActiveUserHabits(int userId);
        public Task<Habit?> GetHabitByIdAsync(int habitId);
        public Task<bool> AddHabitAsync(Habit habit);
        public Task<bool> UpdateHabitAsync(Habit habit);
        public Task<bool> checkUniquenessNameAsync(int userId, int habitId, string habitName);
    }
}
