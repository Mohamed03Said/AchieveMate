using AchieveMate.Models.Enum;
using AchieveMate.Services.IServices;

namespace AchieveMate.Services
{
    public class SessionFactory : ISessionFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SessionFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISessionService GetSessionService(SessionType sessionType)
        {
            return sessionType switch
            {
                SessionType.Stopwatch => _serviceProvider.GetRequiredService<StopwatchService>(),
                SessionType.Timer => _serviceProvider.GetRequiredService<TimerService>(),
                _ => throw new ArgumentException("Invalid session type")
            };

        }
    }
}
