using System.Timers;

namespace AchieveMate.Services;
public class MyTimer
{
    private System.Timers.Timer timer;

    public event Func<object, SessionEventArgs, Task> TimerEnded;

    public TimeSpan Remaining { get; set; }
    public TimeSpan Elapsed { get; set; }
    private readonly int userId;

    public MyTimer(TimeSpan Initial, int userId)
    {
        Remaining = Initial;
        Elapsed = new TimeSpan(0);
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += async (sender, args) => await OnTimedEvent(sender, args);
        timer.AutoReset = true;
        timer.Enabled = false;
        this.userId = userId;
    }

    public void Start()
    {
        timer.Start();
    }

    public void Pause()
    {
        timer.Stop();
    }

    public void Stop()
    {
        timer.Stop();
        Remaining = new TimeSpan(0, 0, 0);
    }

    private async Task OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (Remaining > TimeSpan.FromSeconds(0))
        {
            Remaining = Remaining.Subtract(TimeSpan.FromSeconds(1));
            Elapsed = Elapsed.Add(TimeSpan.FromSeconds(1));
        }

        if (Remaining <= TimeSpan.FromSeconds(0))
        {
            timer.Stop();
            Console.WriteLine("\n\nfrom MyTimer...\n\n");
            if (TimerEnded != null)
            {
                await TimerEnded(this, new SessionEventArgs(userId));
            }
        }
    }

}
