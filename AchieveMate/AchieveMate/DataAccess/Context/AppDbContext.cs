using AchieveMate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<UserDay> UsersDays { get; set; }
        public DbSet<DayHabits> DaysHabits { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<ActivitiesArchive> ActivitiesArchives { get; set; }
        public DbSet<HabitsArchive> HabitsArchives { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = .; Database = AchieveMate; Integrated Security = SSPI; TrustServerCertificate = True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.ApplyConfiguration(new HabitConfiguration()); // not best practice

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
