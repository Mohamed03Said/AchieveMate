using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models.Enum;
using AchieveMate.Services.IServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository, 
            IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<ActivityVM?> GetActivityAsync(int userId, int activityId)
        {
            Activity? activity = await _activityRepository.GetActivityByIdAsync(activityId);
            if(activity == null || activity.UserId != userId)
            {
                return null;
            }
            ActivityVM activityVm = _mapper.Map<ActivityVM>(activity);
            return activityVm;
        }

        public async Task<AllActivitiesVM> GetMyActivitiesAsync(int userId)
        {
            AllActivitiesVM activiteisVM = new ();
            activiteisVM.InProgress = await GetActivitiesByStatusAsync(userId, ActivityStatus.InProgress);
            activiteisVM.ToDo = await GetActivitiesByStatusAsync(userId, ActivityStatus.ToDo);
            activiteisVM.Finished = await GetActivitiesByStatusAsync(userId, ActivityStatus.Finished);

            return activiteisVM;
        }

        public async Task<bool> AddActivityAsync(ActivityVM activityVM, int userId)
        {
            Activity activity = _mapper.Map<Activity>(activityVM);
            activity.UserId = userId;

            bool result = await _activityRepository.AddActivityAsync(activity);

            return result;
        }

        public async Task<bool?> UpdateActivityAsync(ActivityVM activityVM, int activityId, int userId)
        {
            Activity? activity = await _activityRepository.GetActivityByIdAsync(activityId);
            if( activity == null || activity.UserId != userId)
            {
                return null;
            }
            activity = _mapper.Map(activityVM, activity);

            bool result = await _activityRepository.UpdateActivityAsync(activity);

            return result;
        }

        public async Task<List<ActivitiesListVM>> GetActivitiesByStatusAsync(int userId, ActivityStatus status)
        {
            List<ActivitiesListVM> activities =  await _activityRepository.GetActivitiesByStatus(userId, status)
                .Select(a => new ActivitiesListVM
                {
                    Id = a.Id,
                    ExpiryDate = a.ExpiryDate,
                    Name = a.Name,
                    SpentTime = a.SpentTime,
                    FinishedAt = a.EndAt
                })
                .ToListAsync();

            return activities;
        }

        public async Task<bool> IsUniqueActivityName(int userId, int activityId, string activityName)
        {
            bool result = await _activityRepository.checkUniquenessNameAsync(userId, activityId, activityName);
            return result;
        }

    }
}
