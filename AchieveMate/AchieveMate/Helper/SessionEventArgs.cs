namespace AchieveMate.Helper
{
    public class SessionEventArgs : EventArgs
    {
        public int UserId { get; set; }

        public SessionEventArgs(int userId)
        {
            this.UserId = userId;
        }
    }
}
