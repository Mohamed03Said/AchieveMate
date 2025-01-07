using System.ComponentModel.DataAnnotations;

namespace AchieveMate.ViewModels.Account
{
    public class RegisterVM
    {
        [StringLength(maximumLength: 50, ErrorMessage = "Name must be between 3 and 50 characters", MinimumLength = 3)]
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email address is required")]
        //[StringLength(16, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        //[StringLength(255, ErrorMessage = "Password Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
