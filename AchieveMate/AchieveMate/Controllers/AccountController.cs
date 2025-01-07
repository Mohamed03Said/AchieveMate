using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AchieveMate.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _accountService.RegisterUserAsync(registerVM);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "MyDay");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.SignInUserAsync(loginVM);
                if (result)
                {
                    return RedirectToAction("Index", "MyDay");
                }
                ModelState.AddModelError("", "Email and Password are invalid");
            }
            return View(loginVM);
        }

        public IActionResult Logout()
        {
            _accountService.SignOutUserAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<UsersListVM> users = await _accountService.GetAllUsersAsync();
            return View(users);
        }
    }
}
