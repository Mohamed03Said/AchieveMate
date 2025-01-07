using AchieveMate.Models.Enum;

namespace AchieveMate.Services.IServices
{
    public interface ISessionFactory
    {
        public ISessionService GetSessionService(SessionType sessionType);
    }
}
