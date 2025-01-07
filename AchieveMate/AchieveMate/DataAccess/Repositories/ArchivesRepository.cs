using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models;
using AchieveMate.ViewModels.MyDay;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace AchieveMate.DataAccess.Repositories
{
    public class ArchivesRepository : IArchivesRepository
    {
        private readonly AppDbContext _context;
        public ArchivesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDay?> GetUserDayByDateAsync(int userId, DateOnly date)
        {
            UserDay? userDay = await _context.UsersDays
                            .AsNoTracking()
                            .Where(ud => ud.UserId == userId && ud.Date == date)
                            .Include(ud => ud.Sessions)
                            .ThenInclude(s => s.Activity)
                            .Include(ud => ud.Habits)
                            .ThenInclude(h => h.Habit)
                            .FirstOrDefaultAsync();

            return userDay;
        }

        public IQueryable<UserDay> GetUserDays(int userId, int page, int pageSize)
        {
            IQueryable<UserDay> userDays = _context.UsersDays.AsNoTracking().AsQueryable()
                .Where(ud => ud.UserId == userId)
                .OrderByDescending(ud => ud.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return userDays;
        }

        public async Task<int> CountUserDaysAsync(int userId)
        {
            int counter = await _context.UsersDays.CountAsync(ud => ud.UserId == userId);
            return counter;
        }

        public IQueryable<Archive> GetUserArchives(int userId)
        {
            IQueryable<Archive> archives = _context.Archives
                .AsQueryable()
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Include(a => a.Habits)
                .Include(a => a.Activities);

            return archives;
        }

        public async Task<Archive?> GetUserArchiveByYearAsync(int userId, int year)
        {
            Archive? archive = await _context.Archives
                .AsNoTracking()
                .Include(a => a.Habits)
                .Include(a => a.Activities)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Year == year);

            return archive;
        }

        public IQueryable<UserDay> GetDaysOfYear(int userId, int year)
        {
            IQueryable<UserDay> days = _context.UsersDays
             .AsQueryable().AsNoTracking()
             .Where(ud => ud.UserId == userId && ud.Date.Year == year && ud.Date != DateOnly.FromDateTime(DateTime.Today))
             .Include(ud => ud.Habits)
             .ThenInclude(h => h.Habit)
             .Include(ud => ud.Sessions)
             .ThenInclude(s => s.Activity);
            return days;
        }

        public async Task<bool> RemoveDaysOfYearAsync(List<UserDay> days)
        {
            _context.UsersDays.RemoveRange(days);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveSessionsOfYearAsync(ICollection<Session> sessions)
        {
            _context.Sessions.RemoveRange(sessions);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveDaysHabitsOfYearAsync(ICollection<DayHabits> dayHabits)
        {
            _context.DaysHabits.RemoveRange(dayHabits);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddArchiveAsync(Archive archive)
        {
            _context.Archives.Add(archive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeHabitsArchiveAsync(IQueryable<HabitsArchive> habitsArchive)
        {
            _context.HabitsArchives.AddRange(habitsArchive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddHabitArchiveAsync(HabitsArchive habitArchive)
        {
            _context.HabitsArchives.Add(habitArchive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeActivitiesArchiveAsync(IQueryable<ActivitiesArchive> activitiesArchive)
        {
            _context.ActivitiesArchives.AddRange(activitiesArchive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddActivityArchiveAsync(ActivitiesArchive activityArchive)
        {
            _context.ActivitiesArchives.Add(activityArchive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateArchiveAsync(Archive archive)
        {
            _context.Entry(archive).State = EntityState.Modified;
            _context.Archives.Update(archive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateActivitiesArchiveAsync(ActivitiesArchive activitiesArchive)
        {
            _context.ActivitiesArchives.Update(activitiesArchive);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateHabitsArchiveAsync(HabitsArchive habitsArchive)
        {
            _context.Update(habitsArchive);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
