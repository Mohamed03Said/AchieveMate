using AchieveMate.DataAccess.Context;
using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AchieveMate.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManger;
        public AccountRepository(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManger = userManager;
        }

        public async Task<(IdentityResult, AppUser?)> AddAsync(AppUser user, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = await _userManger.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        return (result, null);
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return (result, user);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return (IdentityResult.Failed(new IdentityError { Description = ex.Message }), null);
                }
            }
        }
    }
}
