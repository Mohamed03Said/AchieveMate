using AchieveMate.DataAccess.Repositories.IRepositories;
using AchieveMate.Models;
using AchieveMate.Services.IServices;
using AchieveMate.ViewModels.Account;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Security.Claims;

namespace AchieveMate.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IUserDayRepository _userDayRepository;

        public AccountService(IMapper mapper, 
            IAccountRepository accountRepository, 
            UserManager<AppUser> userManager, 
            ILogger<AccountService> logger, 
            SignInManager<AppUser> signInManager, 
            IUserDayRepository userDayRepository) 
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userDayRepository = userDayRepository;
        }

        [Authorize(Roles ="Admin")]
        public async Task<List<UsersListVM>> GetAllUsersAsync()
        {
            List<UsersListVM> users = await _userManager.Users
                .Select( u => new UsersListVM
                {
                    Name = u.Name,
                    Email = u.Email!,
                    LastLogged = u.LastLogged
                }
                ).ToListAsync();
            return users;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterVM registerVM)
        {
            AppUser user = _mapper.Map<AppUser>(registerVM);

            (IdentityResult, AppUser?) result = await _accountRepository.AddAsync(user, registerVM.Password);

            if (result.Item2 != null)
            {
                try
                {
                    IdentityResult roleResult = await _userManager
                        .AddToRoleAsync(result.Item2, "User");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Can't Assign {"User"} Role to This User\nUserId:{result.Item2.Id}\n {ex.Message.ToString()}");
                }
            }

            return result.Item1;
        }

        public async Task<bool> SignInUserAsync(LoginVM loginVM)
        {
            AppUser? user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user is not null)
            {
                bool result = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (result)
                {
                    int spaceIdx = user.Name.IndexOf(' ');
                    string firstName = spaceIdx != -1 ?  user.Name.Substring(0, spaceIdx) : user.Name;
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.Surname, firstName),
                    };

                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (string role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    await _signInManager.SignInWithClaimsAsync(user, loginVM.RememberMe, claims);

                    return true;
                }
            }
            return false;
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
