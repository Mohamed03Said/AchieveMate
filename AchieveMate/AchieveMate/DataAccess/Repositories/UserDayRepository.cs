using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace AchieveMate.DataAccess.Repositories
{
    public class UserDayRepository : IUserDayRepository
    {
        private readonly AppDbContext _context;

        public UserDayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session.Id > 0;
        }

        public async Task<UserDay> GetOrAddUserDayAsync(int userId)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            UserDay? day = await _context.UsersDays.AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);
            
            if (day == null)
            {
                day = new UserDay
                {
                    UserId = userId
                };
                _context.UsersDays.Add(day);

                await _context.SaveChangesAsync();
            }

            return day;
        }

        public async Task<UserDay> GetUserDayByIdAsync(int dayId)
        {
            UserDay day = await _context.UsersDays.AsNoTracking()
                .FirstAsync(ud => ud.Id == dayId);
            return day;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserDayAsync(UserDay editedUserDay)
        {
            _context.UsersDays.Update(editedUserDay);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddDayHabitAsync(DayHabits dayHabit)
        {
            _context.DaysHabits.Add(dayHabit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<DayHabits>> GetUserDayHabits(int userId, int dayId)
        {
            List<DayHabits> dayHabits = await _context.DaysHabits
                .AsNoTracking()
                .AsQueryable()
                .Include(dh => dh.Habit)
                .Where(dh => dh.DayId == dayId)
                .ToListAsync();
            return dayHabits;
        }

        public async Task<bool> IsDayHabitExistAsync(int dayId, int habitId)
        {
            bool result = await _context.DaysHabits.AsNoTracking()
                .AnyAsync(dh => dh.DayId == dayId && dh.HabitId == habitId);

            return result;
        }

        public async Task<DayHabits?> GetDayHabitAsync(int dayId, int habitId)
        {
            DayHabits? dayHabits = await _context.DaysHabits.AsNoTracking()
                            .Include(dh => dh.Habit)
                            .FirstOrDefaultAsync(dh => dh.HabitId == habitId && dh.DayId == dayId);
            return dayHabits;
        }

        public async Task<bool> UpdateDayHabitStatusAsync(DayHabits dayHabit)
        {
            _context.DaysHabits.Update(dayHabit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSessionAsync(Session session)
        {
            _context.Sessions.Update(session);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveSessionAsync(Session session)
        {
            _context.Sessions.Remove(session);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
