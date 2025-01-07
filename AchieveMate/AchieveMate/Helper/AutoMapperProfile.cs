using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;
using AutoMapper;

namespace AchieveMate.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(scr => scr.Email.Substring(0, scr.Email.IndexOf('@'))));

            CreateMap<ActivityVM, Activity>()
                .ForMember(dist => dist.Id, opt => opt.Ignore())
                .ForMember(dist => dist.SpentTime, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<HabitVM, Habit>()
                .ForMember(dist => dist.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Habit, HabitDetailsVM>()
                .ForMember(dist => dist.MaxStreak, opt => opt.MapFrom(src => Math.Max(src.Streak, src.MaxStreak)));

            CreateMap<Session, SessionVM>();

            CreateMap<UserDay, MyDayVM>();
        }
    }
}
