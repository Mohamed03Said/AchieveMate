using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.Activity;

namespace AchieveMate.DataAccess.Repositories.IRepositories
{
    public interface IActivityRepository
    {
        public IQueryable<Activity> GetAllUserActivities(int userId);
        public Task<Activity?> GetActivityByIdAsync(int activityId);
        public Task<bool> AddActivityAsync(Activity activity);
        public Task<bool> UpdateActivityAsync(Activity activity);
        public IQueryable<Activity> GetActivitiesByStatus(int userId, ActivityStatus status);
        public Task<bool> checkUniquenessNameAsync(int userId, int activityId, string activityName);
    }
}
