using AchieveMate.Services.IServices;
using System.Collections.Concurrent;

namespace AchieveMate.Services
{
    public class TimerService : ISessionService
    {

        private readonly ConcurrentDictionary<int, MyTimer> _sessions;

        public event Func<object, SessionEventArgs, Task> OnFinished;

        public TimerService(ConcurrentDictionary<int, MyTimer> sessions)
        {
            _sessions = sessions;
        }

        public TimeSpan Pause(int sessionId)
        {
            MyTimer timer = _sessions[sessionId];
            timer.Pause();

            return timer.Remaining;
        }

        public TimeSpan Time(int sessionId)
        {
            MyTimer timer = _sessions[sessionId];

            return timer.Remaining;
        }

        public TimeSpan Resume(int sessionId)
        {
            MyTimer timer = _sessions[sessionId];
            timer.Start();

            return timer.Remaining;
        }

        public bool Start(int userId, int sessionId, TimeSpan? Initial)
        {
            MyTimer timer = new(Initial!.Value, userId);
            timer.Start();
            timer.TimerEnded += async(sender, args) => await OnSessionFinished(sender, args);
            _sessions.TryAdd(sessionId, timer);

            return true;
        }

        public async Task OnSessionFinished(object Sender, SessionEventArgs e)
        {
            Console.WriteLine("\n\nfrom MyTImerSer...\n\n");
            await OnFinished(Sender, e);
        }

        public TimeSpan Stop(int sessionId)
        {
            MyTimer timer = _sessions[sessionId];
            timer.Stop();
            _sessions.TryRemove(new KeyValuePair<int, MyTimer>(sessionId, timer));
            return timer.Remaining;
        }

        public TimeSpan Elapsed(int sessionId)
        {
            MyTimer timer = _sessions[sessionId];

            return timer.Elapsed;
        }
    }
}
