using AchieveMate.Helper;
using AchieveMate.Models;
using AchieveMate.Models.Enum;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.MyDay;
using AchieveMate.ViewModels.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.Controllers
{
    [Authorize]
    public class MyDayController : Controller
    {
        private readonly IMyDayService _myDayService;

        public MyDayController(IMyDayService myDayService)
        {
            _myDayService = myDayService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = UserHelper.GetUserId(User);

            MyDayVM myDay = await _myDayService.GetMyDayAsync(userId);

            return View(myDay);
        }

        [HttpPost]
        public IActionResult UpdateSessionStatus()
        {
            int userId = UserHelper.GetUserId(User);
            Session? session = _myDayService.UpdateSessionStatus(userId);
            if (session is not null)
            {
                return Ok(new { status = session.Status.ToString(), type = session.Type.ToString(), time = session.ElapsedTime.TotalSeconds });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMyDay([FromForm] UpdateDayVM updateDayVM)
        {
            if (ModelState.IsValid)
            {
                int userId = UserHelper.GetUserId(User);
                await _myDayService.UpdateMyDayAsync(userId, updateDayVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> StartSession()
        {
            int userId = UserHelper.GetUserId(User);
            StartSessionVM sessionVM = new StartSessionVM();
            sessionVM.Activities = await _myDayService.InProgressActivites(userId)
                                                      .ToListAsync();
            return PartialView("_startSessionPartial", sessionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartSession(StartSessionVM sessionVM)
        {
            int userId = UserHelper.GetUserId(User);
            if (ModelState.IsValid)
            {
                await _myDayService.StartSessionAsync(userId, sessionVM);
                return RedirectToAction("Index");
            }
            sessionVM.Activities = await _myDayService.InProgressActivites(userId)
                                                      .ToListAsync();
            return View("_startSessionPartial", sessionVM);
        }

        [HttpGet]
        public async Task<IActionResult> CancelSession()
        {
            int userId = UserHelper.GetUserId(User);
            bool result = await _myDayService.CancelSession(userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FinishSession()
        {
            int userId = UserHelper.GetUserId(User);
            TimeSpan? time = await _myDayService.FinishSessionAsync(userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateSessionNotes(string editedNotes)
        {
            int userId = UserHelper.GetUserId(User);
            bool result = _myDayService.UpdateSessionNotes(userId, editedNotes);
            if(result == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHabitStatus(int dayId, int habitId, bool status)
        {
            bool result = await _myDayService.UpdateMyHabitStatusAsync(dayId, habitId, status);
            return Ok(new { success = result });
        }
    
    
    }
}
