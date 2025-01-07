using AchieveMate.ViewModels.Archive;
namespace AchieveMate.Services.IServices
{
    public interface IArchivesService
    {
        public Task<PaginationVM<List<DaysListVM>>> GetUserDaysAsync(int userId, int? page);
        public Task<DayDetailsVM?> GetUserDayByDateAsync(int userId, DateOnly date);
        public Task<List<ArchiveYearsListVM>> GetUserYearsArchiveAsync(int userId);
        public Task<ArchiveYearVM?> GetUserYearArchive(int userId, int year);
    }
}
