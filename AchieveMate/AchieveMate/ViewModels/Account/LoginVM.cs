using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email Field is Required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password Field is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
