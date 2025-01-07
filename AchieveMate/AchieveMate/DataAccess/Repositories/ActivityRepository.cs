using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.DataAccess.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Activity> GetAllUserActivities(int userId)
        {
            IQueryable<Activity> activities = _context.Activities
                .AsQueryable().AsNoTracking()
                .Where(g => g.UserId == userId);

            return activities;
        }

        public async Task<Activity?> GetActivityByIdAsync(int activityId)
        {
            Activity? activity = await _context.Activities
                .FirstOrDefaultAsync(g => g.Id == activityId);

            return activity;
        }

        public async Task<bool> AddActivityAsync(Activity activity)
        {
            _context.Activities.Add(activity);

            await _context.SaveChangesAsync();

            return activity.Id > 0;
        }

        public async Task<bool> UpdateActivityAsync(Activity activity)
        {
            _context.Update(activity);

            return await _context.SaveChangesAsync() > 0;
        }

        public IQueryable<Activity> GetActivitiesByStatus(int userId, ActivityStatus status)
        {
            IQueryable<Activity> activites = _context.Activities.AsNoTracking()
                .Where(a => a.UserId == userId && a.Status == status)
                .AsQueryable();
            return activites;
        }

        public async Task<bool> checkUniquenessNameAsync(int userId, int activityId, string activityName)
        {
            bool result = await _context.Activities.AnyAsync(a => a.UserId == userId && 
            a.Id != activityId && 
            a.Name == activityName);

            return result;
        }
    }
}
