using Microsoft.AspNetCore.Identity;

namespace AchieveMate.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public DateTime? LastLogged { get; set; }
        public ICollection<UserDay>? Days { get; set; }
        public ICollection<Activity>? Activities { get; set; }
        public ICollection<Habit>? Habits { get; set; }
        public ICollection<Archive>? Archives { get; set; }
    }
}
