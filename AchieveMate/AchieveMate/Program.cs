using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Services;
using AchieveMate.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace AchieveMate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // register AppDbContext for DI
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("MSSQL"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            );

            /* this lines add by convension with the next AddIdentity
             servicesCollection.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
            servicesCollection.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
            servicesCollection.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
            servicesCollection.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            servicesCollection.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            */

            //register usermanager,rolemanager==>userrole
            builder.Services.AddIdentity<AppUser, IdentityRole<int>>(
                options =>
                {
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }
                ).AddEntityFrameworkStores<AppDbContext>();

            //// to change login redirect path
            //builder.Services.AddAuthentication().AddCookie(options => options.LoginPath = "/Login");

            // register AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // register Repositories
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IHabitRepository, HabitRepository>();
            builder.Services.AddScoped<IUserDayRepository, UserDayRepository>();
            builder.Services.AddScoped<IArchivesRepository, ArchivesRepository>();

            // register Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IHabitService, HabitService>();
            builder.Services.AddScoped<ISessionFactory, SessionFactory>();
            builder.Services.AddScoped<IMyDayService, MyDayService>();
            builder.Services.AddScoped<IArchivesService, ArchivesService>();

            builder.Services.AddScoped<TimerService>();
            builder.Services.AddScoped<StopwatchService>();

            builder.Services.AddSingleton<ConcurrentDictionary<int, Stopwatch>>();
            builder.Services.AddSingleton<ConcurrentDictionary<int, MyTimer>>(); 
            builder.Services.AddSingleton<ConcurrentDictionary<int, Session>>();

            //builder.Services.AddSingleton<IServiceProvider>(sp => sp);


            //Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MyDay}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
