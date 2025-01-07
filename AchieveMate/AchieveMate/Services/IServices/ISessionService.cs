namespace AchieveMate.Services.IServices
{
    public interface ISessionService
    {
        public event Func<object, SessionEventArgs, Task> OnFinished;
        public Task OnSessionFinished(Object Sender,  SessionEventArgs e);
        public bool Start(int userId, int sessionId, TimeSpan? Tinitial);
        public TimeSpan Pause(int sessionId);
        public TimeSpan Stop(int sessionId);
        public TimeSpan Resume(int sessionId);
        public TimeSpan Time(int sessionId);
        public TimeSpan Elapsed(int sessionId);
    }
}
