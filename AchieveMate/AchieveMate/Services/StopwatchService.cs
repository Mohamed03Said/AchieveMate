using AchieveMate.Services.IServices;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace AchieveMate.Services
{
    public class StopwatchService : ISessionService
    {
        private readonly ConcurrentDictionary<int, Stopwatch> _sessions;

        public event Func<object, SessionEventArgs, Task> OnFinished;
        public async Task OnSessionFinished(object Sender, SessionEventArgs e)
        {
            await OnFinished(Sender, e);
        }

        public StopwatchService(ConcurrentDictionary<int, Stopwatch> sessions)
        {
            _sessions = sessions;
        }

        public TimeSpan Time(int sessionId)
        {
            Stopwatch stopwatch = _sessions[sessionId];
            return stopwatch.Elapsed;
        }

        public TimeSpan Pause(int sessionId)
        {
            Stopwatch stopwatch = _sessions[sessionId];
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        public bool Start(int userId, int sessionId, TimeSpan? none)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _sessions.TryAdd(sessionId, stopwatch);

            return true;
        }

        public TimeSpan Stop(int sessionId)
        {
            Stopwatch stopwatch = _sessions[sessionId];
            stopwatch.Stop();
            _sessions.TryRemove(new KeyValuePair<int, Stopwatch>(sessionId, stopwatch));
            return stopwatch.Elapsed;
        }
        public TimeSpan Resume(int sessionId)
        {
            Stopwatch stopwatch = _sessions[sessionId];
            stopwatch.Start();

            return stopwatch.Elapsed;
        }

        public TimeSpan Elapsed(int sessionId)
        {
            return Time(sessionId);
        }
    }
}
