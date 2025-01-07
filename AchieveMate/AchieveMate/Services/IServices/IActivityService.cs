using AchieveMate.Models.Enum;

namespace AchieveMate.Services.IServices
{
    public interface IActivityService
    {
        public Task<AllActivitiesVM> GetMyActivitiesAsync(int userId);
        public Task<ActivityVM?> GetActivityAsync(int userId, int activityId);
        public Task<bool?> UpdateActivityAsync(ActivityVM activityVM, int activityId, int userId);
        public Task<bool> AddActivityAsync(ActivityVM activityVM, int userId);
        public Task<List<ActivitiesListVM>> GetActivitiesByStatusAsync(int userId, ActivityStatus status);
        public Task<bool> IsUniqueActivityName(int userId, int activityId, string activityName);
    }
}
