using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Archive;
using AchieveMate.ViewModels.MyDay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AchieveMate.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly IArchivesService _archivesService;

        public ArchiveController(IArchivesService archivesService)
        {
            _archivesService = archivesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int userId = UserHelper.GetUserId(User);
           var days = await _archivesService.GetUserDaysAsync(userId, page);

            return View(days);
        }

        [HttpGet]
        public async Task<IActionResult> Day(DateOnly date)
        {
            int userId = UserHelper.GetUserId(User);
            DayDetailsVM? dayVM = await _archivesService.GetUserDayByDateAsync(userId, date);

            if(dayVM == null)
            {
                return RedirectToAction("Index");
            }

            return View(dayVM);
        }

        public async Task<IActionResult> Years()
        {
            int userId = UserHelper.GetUserId(User);
            List<ArchiveYearsListVM> yearsArchiveVM = await _archivesService.GetUserYearsArchiveAsync(userId);
            
            return View(yearsArchiveVM);
        }

        public async Task<IActionResult> Year(int year)
        {
            int userId = UserHelper.GetUserId(User);
            ArchiveYearVM? yearVM = await _archivesService.GetUserYearArchive(userId, year);

            if (yearVM == null)
            {
                return RedirectToAction("Years");
            }

            return View(yearVM);
        }
    }
}
