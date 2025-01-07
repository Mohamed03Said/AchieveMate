using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.ViewModels.Habit;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.DataAccess.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly AppDbContext _context;

        public HabitRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Habit> GetAllUserHabits(int userId)
        {
            IQueryable<Habit> habits = _context.Habits
                .AsQueryable().AsNoTracking()
                .Where(h => h.UserId == userId);

            return habits;
        }
        public IQueryable<Habit> GetActiveUserHabits(int userId)
        {
            IQueryable<Habit> habits = _context.Habits
                .AsQueryable().AsNoTracking()
                .Where(h => h.UserId == userId && h.Type != Models.Enum.HabitType.InActive);

            return habits;
        }

        public async Task<Habit?> GetHabitByIdAsync(int habitId)
        {
            Habit? habit = await _context.Habits.AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == habitId);

            return habit;
        }

        public async Task<bool> AddHabitAsync(Habit habit)
        {
            _context.Habits.Add(habit);

            await _context.SaveChangesAsync();

            return habit.Id > 0;
        }

        public async Task<bool> UpdateHabitAsync(Habit habit)
        {
            _context.Habits.Update(habit);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> checkUniquenessNameAsync(int userId, int habitId, string habitName)
        {
            bool result = await _context.Habits.AnyAsync(h => h.UserId == userId &&
            h.Id != habitId &&
            h.Name == habitName);

            return result;
        }
    }
}
