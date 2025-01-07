using AchieveMate.Helper;
using AchieveMate.Services;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Habit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AchieveMate.Controllers
{
    [Authorize]
    public class HabitController : Controller
    {
        private readonly IHabitService _habitService;

        public HabitController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = UserHelper.GetUserId(User);
            List<HabitsListVM> habits = await _habitService.GetMyHabitsAsync(userId);
            return View(habits);
        }

        #region Remote Validations
        public async Task<JsonResult> HabitValidation(string Name, int Id)
        {
            int userId = UserHelper.GetUserId(User);
            bool result = await _habitService.IsUniqueHabitName(userId, Id, Name);
            return Json(!result);
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            int userId = UserHelper.GetUserId(User);
            HabitDetailsVM? habit = await _habitService.GetHabitAsync(userId, id);
            if (habit == null)
            {
                return NotFound();
            }
            return View(habit);
        }

        [HttpGet]
        public ActionResult Create()
        {
            HabitVM habitVM = new HabitVM();
            return View(habitVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HabitVM habitVM)
        {
            if (ModelState.IsValid)
            {
                int userId = UserHelper.GetUserId(User);

                bool result = await _habitService.AddHabitAsync(habitVM, userId);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can't Create Your New Habit, Please try again Later");
                }
            }
            return View(habitVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            int userId = UserHelper.GetUserId(User);
            HabitDetailsVM? habit = await _habitService.GetHabitAsync(userId, id);
            if (habit != null)
            {
                HabitVM habitVM = new HabitVM { Name = habit.Name, Notes = habit.Notes??"", Type = habit.Type };
                return View(habitVM);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(HabitVM habitVM, int id)
        {
            if (ModelState.IsValid)
            {
                int userId = UserHelper.GetUserId(User);

                bool? result = await _habitService.UpdateHabitAsync(habitVM, id, userId);

                if (result == null)
                {
                    return NotFound();
                }
                else if (result == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can't Update Your Habit, Please try again Later");
                }
            }
            return View(habitVM);
        }
    }
}
