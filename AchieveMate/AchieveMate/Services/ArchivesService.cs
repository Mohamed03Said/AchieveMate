using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Archive;
using AchieveMate.ViewModels.MyDay;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.Services
{
    public class ArchivesService : IArchivesService
    {
        private readonly IArchivesRepository _archivesRepository;
        private readonly IMapper _mapper;
        public ArchivesService(IArchivesRepository archivesRepository, 
            IMapper mapper)
        {
            _archivesRepository = archivesRepository;
            _mapper = mapper;
    }

        public async Task<PaginationVM<List<DaysListVM>>> GetUserDaysAsync(int userId, int? page)
        {
            int pageSize = 7;
            List<DaysListVM> daysVM =  await _archivesRepository.GetUserDays(userId, page ?? 1, pageSize)
                .Select(ud => new DaysListVM
                {
                    Date = ud.Date,
                    Evaluation = ud.Evaluation,
                    Achievement = ud.Achievement
                })
                .ToListAsync();

            double total = await _archivesRepository.CountUserDaysAsync(userId);
            PaginationVM<List<DaysListVM>> days = new()
            {
                instance = daysVM,
                Page = page ?? 1,
                TotalPages = (int)Math.Ceiling(total / pageSize)
            };

            return days;
        }

        public async Task<DayDetailsVM?> GetUserDayByDateAsync(int userId, DateOnly date)
        {
            UserDay? userDay =  await _archivesRepository.GetUserDayByDateAsync(userId, date);
            if (userDay == null)
            {
                return null;
            }

            DayDetailsVM dayVM = new DayDetailsVM
            {
                Habits = userDay.Habits.Select(h => new DayHabitsListVM
                {
                    IsCompleted = h.IsCompleted,
                    Habit = h.Habit.Name,
                    Type = h.Habit.Type,
                }).ToList(),
                Sessions = userDay.Sessions.Select(s => new SessionsListVM
                {
                    Session = s,
                    Activity = s.Activity.Name
                }).ToList(),

                Date = userDay.Date,
                Evaluation = userDay.Evaluation,
                Notes = userDay.Notes,
                Achievement = userDay.Achievement,
            };

            return dayVM;
        }

        public async Task<List<ArchiveYearsListVM>> GetUserYearsArchiveAsync(int userId)
        {
            List<ArchiveYearsListVM>? archiveListVM = await _archivesRepository
                .GetUserArchives(userId).Select(a => new ArchiveYearsListVM
                {
                    Year = a.Year,
                    Achievement = a.Achievement,
                    CompletedHabits = a.CompletedHabits
                }).ToListAsync();

            return archiveListVM;
        }

        private async Task<bool> RemoveArchiveDependecies(List<UserDay> days)
        {
            bool result = true;
            foreach (UserDay day in days)
            {
                result = result & await _archivesRepository.RemoveDaysHabitsOfYearAsync(day.Habits);
                result = result & await _archivesRepository.RemoveSessionsOfYearAsync(day.Sessions);
            }
            result = result & await _archivesRepository.RemoveDaysOfYearAsync(days);

            return result;
        }

        private async Task<Archive> MergerArchiveYear(Archive archive, IQueryable<UserDay> days)
        {
            var sessionsGroups = await days 
                .SelectMany(d => d.Sessions)
                .GroupBy(d => d.ActivityId)
                .ToListAsync();

            foreach (var sessions in sessionsGroups)
            {
                string activityName = sessions.First().Activity.Name;
                ActivitiesArchive? existActivity = archive.Activities.FirstOrDefault(a => a.Name == activityName);

                if (existActivity is not null)
                {
                    existActivity.TotalSeconds += sessions.Sum(s => s.TotalSeconds);
                    existActivity.Sessions += sessions.Count();
                    existActivity.IsFinished = sessions.First().Activity.Status == Models.Enum.ActivityStatus.Finished;

                    await _archivesRepository.UpdateActivitiesArchiveAsync(existActivity);
                }
                else
                {
                    ActivitiesArchive newActivityArchive = new()
                    {
                        ArchiveId = archive.Id,
                        IsFinished = sessions.First().Activity.Status == Models.Enum.ActivityStatus.Finished,
                        Name = sessions.First().Activity.Name,
                        Sessions = sessions.Count(),
                        TotalSeconds = sessions.Sum(s => s.TotalSeconds)
                    };

                    await _archivesRepository.AddActivityArchiveAsync(newActivityArchive);
                }
            }

            var daysHabitsGroups = await days
                .SelectMany(d => d.Habits)
                .GroupBy(d => d.HabitId)
                .ToListAsync();

            foreach (var daysHabits in daysHabitsGroups)
            {
                string habitName = daysHabits.First().Habit.Name;
                HabitsArchive? existHabit = archive.Habits.FirstOrDefault(h => h.Name == habitName);

                if (existHabit is not null)
                {
                    existHabit.Achievement += daysHabits.Count(dh => dh.IsCompleted);
                    existHabit.IsActive = daysHabits.First().Habit.Type != Models.Enum.HabitType.InActive;

                    await _archivesRepository.UpdateHabitsArchiveAsync(existHabit);
                }
                else
                {
                    HabitsArchive newHabitArchive = new()
                    {
                        Achievement = daysHabits.Count(h => h.IsCompleted),
                        Name = daysHabits.First().Habit.Name,
                        ArchiveId = archive.Id,
                        IsActive = daysHabits.First().Habit.Type != Models.Enum.HabitType.InActive
                    };

                    await _archivesRepository.AddHabitArchiveAsync(newHabitArchive);
                }
            }

            archive.TotalSeconds += days.Sum(d => d.TotalSeconds);
            archive.CompletedHabits += await days
            .Where(d => d.Habits != null)
            .SelectMany(d => d.Habits)
            .CountAsync(h => h.IsCompleted);

            await _archivesRepository.UpdateArchiveAsync(archive);

            return archive;
        }

        private async Task<Archive> GenerateArchiveYear(Archive archive, IQueryable<UserDay> days)
        {
            IQueryable<ActivitiesArchive> activitiesArchive = days.SelectMany(d => d.Sessions)
                .GroupBy(s => s.ActivityId)
                .Select(sessions => new ActivitiesArchive
                {
                    ArchiveId = archive.Id,
                    IsFinished = sessions.First().Activity.Status == Models.Enum.ActivityStatus.Finished,
                    Name = sessions.First().Activity.Name,
                    Sessions = sessions.Count(),
                    TotalSeconds = sessions.Sum(s => s.TotalSeconds)
                });

            await _archivesRepository.AddRangeActivitiesArchiveAsync(activitiesArchive);

            IQueryable<HabitsArchive> habitsArchive = days.SelectMany(d => d.Habits)
                .GroupBy(h => h.HabitId)
                .Select(daysHabits => new HabitsArchive
                {
                    Achievement = daysHabits.Count(h => h.IsCompleted),
                    Name = daysHabits.First().Habit.Name,
                    ArchiveId = archive.Id,
                    IsActive = daysHabits.First().Habit.Type != Models.Enum.HabitType.InActive
                });

            await _archivesRepository.AddRangeHabitsArchiveAsync(habitsArchive);

            archive.TotalSeconds = days.Sum(d => d.TotalSeconds);
            archive.CompletedHabits += await days
            .Where(d => d.Habits != null)
            .SelectMany(d => d.Habits)
            .CountAsync(h => h.IsCompleted);
            await _archivesRepository.UpdateArchiveAsync(archive);

            return archive;
        }

        private async Task<Archive?> GetOrGenerateArchiveYearAsync(int userId, int year)
        {
            Archive? archive = await _archivesRepository.GetUserArchiveByYearAsync(userId, year);
            IQueryable<UserDay> days = _archivesRepository.GetDaysOfYear(userId, year);
            

            if (archive is null)
            {
                if (days.Count() == 0)
                    return null;

                Archive newAarchive = new Archive
                {
                    Year = year,
                    UserId = userId,
                    LastDate = days?.Max(d => d.Date)
                };
                await _archivesRepository.AddArchiveAsync(newAarchive);
                await GenerateArchiveYear(newAarchive, days);
                //await RemoveArchiveDependecies(await days.ToListAsync());

                return newAarchive;
            }
            if (days.Count() > 0)
            {
                days = days.Where(d => d.Date > archive.LastDate);
                await MergerArchiveYear(archive, days);
                //await RemoveArchiveDependecies(await days.ToListAsync());
            }

            return archive;
        }

        public async Task<ArchiveYearVM?> GetUserYearArchive(int userId, int year)
        {
            Archive? archive = await GetOrGenerateArchiveYearAsync(userId, year);
            if(archive is null)
            {
                return null;
            }

            ArchiveYearVM archiveYearVM = new ArchiveYearVM
            {
                Year = archive.Year,
                AchieveMent = archive.Achievement,
                CompletedHabits = archive.CompletedHabits,
                Habits = archive.Habits?.Select(h => new HabitArchiveVM
                {
                    Completed = h.Achievement,
                    IsActive = h.IsActive,
                    Name = h.Name
                }).ToList(),
                Activities = archive.Activities?.Select(a => new ActivityArchiveVM
                {
                    SpentTime = a.SpentTime,
                    Sessions = a.Sessions,
                    IsFinished = a.IsFinished,
                    Name = a.Name
                }).ToList()
            };

            return archiveYearVM;
        }
    }
}
