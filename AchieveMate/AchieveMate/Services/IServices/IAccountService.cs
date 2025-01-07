using AchieveMate.Models;
using AchieveMate.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace AchieveMate.Services.IServices
{
    public interface IAccountService
    {
        public Task<IdentityResult> RegisterUserAsync(RegisterVM registerVM);
        public Task<bool> SignInUserAsync(LoginVM loginVM);
        public Task SignOutUserAsync();
        public Task<List<UsersListVM>> GetAllUsersAsync();
    }
}
