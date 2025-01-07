using AchieveMate.Models;
using Microsoft.AspNetCore.Identity;

namespace AchieveMate.DataAccess.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        public Task<(IdentityResult, AppUser?)> AddAsync(AppUser user, string password);
    }
}
