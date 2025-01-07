using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.Session;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.Attributes
{
    public class TimerSessionValidations : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is StartSessionVM startSessionVM)
            {
                TimeSpan? timer = startSessionVM.InitialTimer;
                SessionType type = startSessionVM.Type;

                if(type == SessionType.Timer)
                {
                    if (timer.HasValue)
                    {
                        if(timer.Value <= new TimeSpan(0, 0, 0))
                        {
                            return new ValidationResult("InValid Timer");
                        }
                    }
                    else
                    {
                        return new ValidationResult("Timer Session Fields are Required");
                    }
                }

                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid Usage");
        }
    }
}
