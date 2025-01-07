using System.Security.Claims;

namespace AchieveMate.Helper
{
    public static class UserHelper
    {
        public static int GetUserId(ClaimsPrincipal User)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return userId;
        }

        public static string GetUserFirstName(ClaimsPrincipal User)
        {
            string firstName = User.FindFirstValue(ClaimTypes.Surname)!;
            return firstName;
        }

        public static string GetUsername(ClaimsPrincipal User)
        {
            string firstName = User.FindFirstValue(ClaimTypes.Name)!;
            return firstName;
        }
    }
}
