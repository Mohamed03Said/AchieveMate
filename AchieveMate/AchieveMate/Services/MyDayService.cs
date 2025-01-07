using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models;
using AchieveMate.Models.Enum;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace AchieveMate.Services
{
    public class MyDayService : IMyDayService
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ConcurrentDictionary<int, Session> _sessions;
        private readonly ConcurrentDictionary<int, MyTimer> _timerSessions;
        private readonly IUserDayRepository _userDayRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IHabitRepository _habitRepository;

        public MyDayService(ISessionFactory sessionFactory, 
            ConcurrentDictionary<int, Session> sessions, 
            ConcurrentDictionary<int, MyTimer> timerSessions,
            IUserDayRepository userDayRepository,
            IActivityRepository activityRepository, 
            IHabitRepository habitRepository)
        {
            _sessionFactory = sessionFactory;
            _sessions = sessions;
            _timerSessions = timerSessions;
            _userDayRepository = userDayRepository;
            _activityRepository = activityRepository;
            _habitRepository = habitRepository;
        }

        #region Habits
        public async Task<bool> UpdateMyHabitStatusAsync(int dayId, int habitId, bool isCompleted)
        {
            DayHabits? dayHabits = await _userDayRepository.GetDayHabitAsync(dayId, habitId);

            if (dayHabits == null)
            {
                return false;
            }

            dayHabits.IsCompleted = isCompleted;

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            if (isCompleted)
            {
                dayHabits.Habit.Completed++;
                dayHabits.Habit.Streak++;
                dayHabits.Habit.LastCompletedDate = today;
                if (dayHabits.Habit.StreakDate == null)
                {
                    dayHabits.Habit.StreakDate = today;
                }
            }
            else
            {
                dayHabits.Habit.LastCompletedDate = null;
                dayHabits.Habit.Streak--;
                dayHabits.Habit.Completed --;
            }

            bool result = await _userDayRepository.UpdateDayHabitStatusAsync(dayHabits);
            return result;
        }

        private Habit HandleHabitCases(Habit habit)
        {
            habit.MaxStreak = Math.Max(habit.Streak, habit.MaxStreak);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly yesterday = today.AddDays(-1);
            if (habit.LastCompletedDate == null || habit.LastCompletedDate != yesterday)
            {
                habit.LastCompletedDate = null;
                habit.StreakDate = today;
                habit.Streak = 0;
            }
            return habit;
        }

        #endregion

        #region Session

        /*public async Task<TimeSpan?> FinishSessionAsync(int userId)
        {
            Console.WriteLine("\n\nenter\n\n");
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Console.WriteLine("\n\ncooking\n\n");
                Session session = _sessions[userId];
                Console.WriteLine("\n\ncooking0.1\n\n");
                Console.WriteLine($"\n\n{session.Type}.1\n\n");

                Console.WriteLine("\n\ncooking1\n\n");
                TimeSpan time;

                var serviceCollection = new ServiceCollection();

                serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server = .; Database = AchieveMate; Integrated Security = SSPI; TrustServerCertificate = True;"));

                serviceCollection.AddScoped<IUserDayRepository, UserDayRepository>();
                serviceCollection.AddScoped<TimerService>();
                serviceCollection.AddSingleton<ConcurrentDictionary<int, MyTimer>>();

                var serviceProvider = serviceCollection.BuildServiceProvider();

                Console.WriteLine("\n\ncooking2\n\n");
                using (var scope = serviceProvider.CreateScope())
                {

                    Console.WriteLine("\n\ncooking2.1\n\n");
                    IUserDayRepository userDayRepository = scope.ServiceProvider.GetRequiredService<IUserDayRepository>();

                    Console.WriteLine("\n\ncooking2.2\n\n");
                    //var mytimerService = scope.ServiceProvider.GetRequiredService<TimerService>();
                    var mytimerService = new TimerService(_timerSessions);

                    Console.WriteLine("\n\ncooking0.2\n\n");
                    TimeSpan elapsed = mytimerService.Elapsed(session.Id);
                    Console.WriteLine("\n\ncooking0.3\n\n");
                    time = mytimerService.Stop(session.Id);
                    Console.WriteLine("\n\ncooking0.4\n\n");
                    session.Status = SessionStatus.Finished;
                    Console.WriteLine("\n\ncooking0.5\n\n");
                    session.FinishedAt = DateTime.Now;
                    Console.WriteLine("\n\ncooking0.6\n\n");
                    session.ElapsedTime = elapsed;
                    Console.WriteLine("\n\ncooking0.7\n\n");
                    session.PauseTime = (session.FinishedAt.Value - session.StartAt) - session.ElapsedTime;
                    await userDayRepository.UpdateSessionAsync(session);
                }

                _sessions.TryRemove(new KeyValuePair<int, Session>(userId, session));

                Console.WriteLine("\n\ncooking3\n\n");
                // add elapsed time to the activity and the day
                await HandleSessionDependenciesAsync(session);
                Console.WriteLine("\n\ncooking4\n\n");

                return time;
            }
            return null;
        }*/

        private async Task OnSessionFinished(object Sender, SessionEventArgs e)
        {
            Console.WriteLine($"\n\nMyDaySer.with {e.UserId} ......\n\n\n");
            await FinishTimerSessionAsync(e.UserId);
        }

        public async Task<TimeSpan?> FinishTimerSessionAsync(int userId)
        {
            Console.WriteLine("\n\nenter\n\n");
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Console.WriteLine("\n\ncooking\n\n");
                Session session = _sessions[userId];
                Console.WriteLine("\n\ncooking0.1\n\n");
                Console.WriteLine($"\n\n{session.Type}.1\n\n");

                TimeSpan time;
                using (var context = new AppDbContext())
                {

                    Console.WriteLine("\n\ncooking2.1\n\n");
                    IUserDayRepository userDayRepository = new UserDayRepository(context);
                    IActivityRepository activityRepository = new ActivityRepository(context);
                    ISessionService mytimerService = new TimerService(_timerSessions);

                    Console.WriteLine("\n\ncooking0.2\n\n");
                    TimeSpan elapsed = mytimerService.Elapsed(session.Id);
                    Console.WriteLine("\n\ncooking0.3\n\n");
                    time = mytimerService.Stop(session.Id);
                    Console.WriteLine("\n\ncooking0.4\n\n");
                    session.Status = SessionStatus.Finished;
                    Console.WriteLine("\n\ncooking0.5\n\n");
                    session.FinishedAt = DateTime.Now;
                    Console.WriteLine("\n\ncooking0.6\n\n");
                    session.ElapsedTime = elapsed;
                    session.PauseTime = (session.FinishedAt.Value - session.StartAt) - session.ElapsedTime;

                    await userDayRepository.UpdateSessionAsync(session);

                    Console.WriteLine("\n\ncooking0.7\n\n");
                    _sessions.TryRemove(new KeyValuePair<int, Session>(userId, session));

                    Console.WriteLine("\n\ncooking3\n\n");
                    /////

                    UserDay day = await userDayRepository.GetUserDayByIdAsync(session.DayId);
                    Activity? activity = await activityRepository.GetActivityByIdAsync(session.ActivityId);

                    day.Achievement += session.ElapsedTime;
                    activity!.SpentTime += session.ElapsedTime;

                    await activityRepository.UpdateActivityAsync(activity);
                    await userDayRepository.UpdateUserDayAsync(day);

                }
                ////
                Console.WriteLine("\n\ncooking4\n\n");

                return time;
            }
            return null;
        }

        private async Task HandleSessionDependenciesAsync(Session session)
        {
            UserDay day = await _userDayRepository.GetUserDayByIdAsync(session.DayId);
            Activity activity = await _activityRepository.GetActivityByIdAsync(session.ActivityId);

            day.Achievement += session.ElapsedTime;
            activity!.SpentTime += session.ElapsedTime;

            await _activityRepository.UpdateActivityAsync(activity);
            await _userDayRepository.UpdateUserDayAsync(day);
        }

        private TimeSpan PauseSession(Session session, ISessionService sessionservice)
        {
            TimeSpan time = sessionservice.Pause(session.Id);
            TimeSpan elapsed = sessionservice.Elapsed(session.Id);
            session.Status = SessionStatus.Paused;
            session.ElapsedTime = elapsed;
            //await _userDayRepository.UpdateSessionAsync(session);
            return time;
        }

        private TimeSpan ResumeSession(Session session, ISessionService sessionservice)
        {
            TimeSpan time = sessionservice.Resume(session.Id);
            TimeSpan elapsed = sessionservice.Elapsed(session.Id);
            session.Status = SessionStatus.InProgress;
            session.ElapsedTime = elapsed;
            //await _userDayRepository.UpdateSessionAsync(session);
            return time;
        }

        public async Task<TimeSpan?> FinishSessionAsync(int userId)
        {
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Session session = _sessions[userId];
                ISessionService _sessionService = _sessionFactory.GetSessionService(session.Type);
                TimeSpan elapsed = _sessionService.Elapsed(session.Id);
                TimeSpan time = _sessionService.Stop(session.Id);
                session.Status = SessionStatus.Finished;
                session.FinishedAt = DateTime.Now;
                session.ElapsedTime = elapsed;
                session.PauseTime = (session.FinishedAt.Value - session.StartAt) - session.ElapsedTime;

                await _userDayRepository.UpdateSessionAsync(session);

                _sessions.TryRemove(new KeyValuePair<int, Session>(userId, session));

                await HandleSessionDependenciesAsync(session);

                return time;
            }
            return null;
        }

        public async Task<bool> CancelSession(int userId)
        {
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Session session = _sessions[userId];
                ISessionService sessionservice = _sessionFactory.GetSessionService(session.Type);
                TimeSpan time = sessionservice.Stop(session.Id);
                bool result = await _userDayRepository.RemoveSessionAsync(session);
                result = result & _sessions.TryRemove(new KeyValuePair<int, Session>(userId, session));

                return result;
            }
            return false;
        }

        public Session? UpdateSessionStatus(int userId)
        {
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Session session = _sessions[userId];
                ISessionService sessionservice = _sessionFactory.GetSessionService(session.Type);
                if(session.Status == SessionStatus.InProgress)
                {
                    session.ElapsedTime = PauseSession(session, sessionservice);
                }
                else if(session.Status == SessionStatus.Paused)
                {
                    session.ElapsedTime = ResumeSession(session, sessionservice);
                }
                return session;
            }
            return null;
        }
        
        private async Task<Session> AddSessionAsync(int dayId, StartSessionVM sessionVM)
        {
            Session session = new Session
            {
                StartAt = DateTime.Now,
                DayId = dayId,
                Notes = sessionVM.Notes,
                Type = sessionVM.Type,
                ActivityId = sessionVM.Activity,
            };

            if(sessionVM.Type == SessionType.Recorded)
            {
                session.ElapsedTime = sessionVM.ElapsedTime!.Value;
                session.StartAt = sessionVM.StartAt!.Value;
                session.Status = SessionStatus.Finished;
                session.FinishedAt = sessionVM.StartAt.Value + sessionVM.ElapsedTime;

                await HandleSessionDependenciesAsync(session);
            }
            await _userDayRepository.AddSessionAsync(session);

            return session;
        }

        public async Task<bool> StartSessionAsync(int userId, StartSessionVM sessionVM)
        {
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist != true)
            {
                UserDay day = await _userDayRepository.GetOrAddUserDayAsync(userId);
                Session session = await AddSessionAsync(day.Id, sessionVM);
                if (session.Type == SessionType.Recorded)
                {
                    return true;
                }
                _sessions.TryAdd(userId, session);
                ISessionService sessionservice = _sessionFactory.GetSessionService(sessionVM.Type);
                sessionservice.OnFinished += async (sender, args) => await OnSessionFinished(sender, args);
                sessionservice.Start(userId, session.Id, sessionVM.InitialTimer);
                return true;
            }
            return false;
        }

        public bool UpdateSessionNotes(int userId, string notes)
        {
            bool isExist = _sessions.ContainsKey(userId);
            if (isExist == true)
            {
                Session session = _sessions[userId];
                session.Notes = notes;
                //result = await _userDayRepository.UpdateSessionAsync(session);
                return true;
            }
            return false;
        }

        #endregion

        #region MyDay
        public IQueryable<Activity> InProgressActivites(int userId)
        {
            IQueryable<Activity> inProgressActivities = _activityRepository
                .GetActivitiesByStatus(userId, ActivityStatus.InProgress);
            return inProgressActivities;
        }
        public async Task<bool> UpdateMyDayAsync(int userId, UpdateDayVM updateDayVM)
        {
            UserDay day = await _userDayRepository.GetOrAddUserDayAsync(userId);

            day.Evaluation = updateDayVM.Evaluation;
            day.Notes = updateDayVM.Notes;

            bool result = await _userDayRepository.UpdateUserDayAsync(day);

            return result;
        }

        public async Task<MyDayVM> GetMyDayAsync(int userId)
        {
            UserDay userDay = await _userDayRepository
                .GetOrAddUserDayAsync(userId);

            List<Habit> habits = await _habitRepository
                .GetActiveUserHabits(userId).ToListAsync();

            foreach (Habit habit in habits)
            {
                bool isExist = await _userDayRepository.IsDayHabitExistAsync(userDay.Id, habit.Id);
                if(isExist == false)
                {
                    DayHabits dayHabit = new DayHabits
                    {
                        DayId = userDay.Id,
                        HabitId = habit.Id
                    };
                    await _userDayRepository.AddDayHabitAsync(dayHabit);
                    
                    HandleHabitCases(habit);
                    
                    await _habitRepository.UpdateHabitAsync(habit);
                } 
            }

            List<DayHabits> dayHabits = await _userDayRepository
                .GetUserDayHabits(userId, userDay.Id);

            MyDayVM myDayVM = new MyDayVM
            {
                Habits = dayHabits,
                Date = userDay.Date,
                Achievement = userDay.Achievement,
                Evaluation = userDay.Evaluation,
                Notes = userDay.Notes
            };

            if (_sessions.ContainsKey(userId))
            {
                Session session = _sessions[userId];
                Activity activity = await _activityRepository.GetActivityByIdAsync(_sessions[userId].ActivityId);
                ISessionService sessionService = _sessionFactory.GetSessionService(session.Type);
                
                myDayVM.Session = new SessionVM
                {
                    Activity = activity!.Name,
                    Notes = session.Notes,
                    Status = session.Status,
                    Type = session.Type,
                    Time = sessionService.Time(session.Id)
                };
            }

            return myDayVM;
        }

        #endregion

    }
}
