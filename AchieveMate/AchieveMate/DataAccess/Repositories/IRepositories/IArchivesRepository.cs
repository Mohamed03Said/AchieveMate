using AchieveMate.ViewModels.MyDay;

namespace AchieveMate.DataAccess.Repositories.IRepositories
{
    public interface IArchivesRepository
    {
        public IQueryable<UserDay> GetUserDays(int userId, int page, int pageSize);
        public Task<UserDay?> GetUserDayByDateAsync(int userId, DateOnly daty);
        public Task<int> CountUserDaysAsync(int userId);
        public IQueryable<Archive> GetUserArchives(int userId);
        public Task<Archive?> GetUserArchiveByYearAsync(int userId, int year);
        public IQueryable<UserDay> GetDaysOfYear(int userId, int year);
        public Task<bool> RemoveDaysOfYearAsync(List<UserDay> days);
        public Task<bool> RemoveSessionsOfYearAsync(ICollection<Session> sessions);
        public Task<bool> RemoveDaysHabitsOfYearAsync(ICollection<DayHabits> dayHabits);
        public Task<bool> AddArchiveAsync(Archive archive);
        public Task<bool> UpdateArchiveAsync(Archive archive);
        public Task<bool> AddRangeHabitsArchiveAsync(IQueryable<HabitsArchive> habitsArchive);
        public Task<bool> AddRangeActivitiesArchiveAsync(IQueryable<ActivitiesArchive> activitiesArchive);
        public Task<bool> AddActivityArchiveAsync(ActivitiesArchive activityArchive);
        public Task<bool> AddHabitArchiveAsync(HabitsArchive habitArchive);
        public Task<bool> UpdateActivitiesArchiveAsync(ActivitiesArchive activitiesArchive);
        public Task<bool> UpdateHabitsArchiveAsync(HabitsArchive habitsArchive);
    }
}
