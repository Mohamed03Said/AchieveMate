using AchieveMate.Helper;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AchieveMate.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _Activitieservice;

        public ActivityController(IActivityService Activitieservice)
        {
            _Activitieservice = Activitieservice;
        }

        #region Remote Validations
        public async Task<JsonResult> ActivityValidation(string Name, int Id)
        {
            int userId = UserHelper.GetUserId(User);
            bool result = await _Activitieservice.IsUniqueActivityName(userId, Id, Name);
            return Json(!result);
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            int userId = UserHelper.GetUserId(User);
            AllActivitiesVM Activities = await _Activitieservice.GetMyActivitiesAsync(userId);
            return View(Activities);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            int userId = UserHelper.GetUserId(User);
            ActivityVM? activity = await _Activitieservice.GetActivityAsync(userId, id);
            if(activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ActivityVM activityVM)
        {
            if (ModelState.IsValid)
            {
                int userId = UserHelper.GetUserId(User);

                bool result = await _Activitieservice.AddActivityAsync(activityVM, userId);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can't Create Your New Activity, Please try again Later");
                }
            }
            return View(activityVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            int userId = UserHelper.GetUserId(User);
            ActivityVM? activityVM = await _Activitieservice.GetActivityAsync(userId, id);
            if(activityVM != null)
            {
                return View(activityVM);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ActivityVM activityVM, int id)
        {
            if (ModelState.IsValid)
            {
                int userId = UserHelper.GetUserId(User);

                bool? result = await _Activitieservice.UpdateActivityAsync(activityVM, id, userId);

                if (result == null)
                {
                    return NotFound();
                }
                else if(result == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can't Update Your Activity, Please try again Later");
                }
            }
            return View(activityVM);
        }
    }
}
