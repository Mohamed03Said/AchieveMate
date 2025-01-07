using AchieveMate.DataAccess.Repositories;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Services.IServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.Services
{
    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IMapper _mapper;

        public HabitService(IHabitRepository habitRepository,
            IMapper mapper)
        {
            _habitRepository = habitRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddHabitAsync(HabitVM habitVM, int userId)
        {
            Habit habit = _mapper.Map<Habit>(habitVM);
            habit.UserId = userId;

            bool result = await _habitRepository.AddHabitAsync(habit);

            return result;
        }

        public async Task<HabitDetailsVM?> GetHabitAsync(int userId, int habitId)
        {
            Habit? habit = await _habitRepository.GetHabitByIdAsync(habitId);
            // check if habit exist and belong to this user
            if (habit == null || habit.UserId != userId)
            {
                return null;
            }
            HabitDetailsVM habitVM = _mapper.Map<HabitDetailsVM>(habit);
            return habitVM;
        }

        public async Task<List<HabitsListVM>> GetMyHabitsAsync(int userId)
        {
            List<HabitsListVM> habitsList = await _habitRepository
                .GetAllUserHabits(userId).Select(h => new HabitsListVM
                {
                    Id = h.Id,
                    Name = h.Name,
                    Streak = h.Streak,
                    Type = h.Type,
                })
                .ToListAsync();

            return habitsList;
        }

        public async Task<bool?> UpdateHabitAsync(HabitVM habitVM, int habitId, int userId)
        {
            Habit? habit = await _habitRepository.GetHabitByIdAsync(habitId);
            // check if habit exist and belong to this user
            if (habit == null || habit.UserId != userId)
            {
                return null;
            }
            habit = _mapper.Map(habitVM, habit);

            bool result = await _habitRepository.UpdateHabitAsync(habit);

            return result;
        }
        public async Task<bool> IsUniqueHabitName(int userId, int habitId, string habitName)
        {
            bool result = await _habitRepository.checkUniquenessNameAsync(userId, habitId, habitName);
            return result;
        }
    }
}
