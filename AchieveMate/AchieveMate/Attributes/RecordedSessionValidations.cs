using AchieveMate.Models.Enum;
using AchieveMate.ViewModels.Session;
using System.ComponentModel.DataAnnotations;

namespace AchieveMate.Attributes
{
    public class RecordedSessionValidations : ValidationAttribute
    {
        public string Msg { get; set; } = null!;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(validationContext.ObjectInstance is StartSessionVM startSessionVM)
            {
                DateTime? startAt = startSessionVM.StartAt;
                TimeSpan? elpasedTime = startSessionVM.ElapsedTime;
                DateTime now = DateTime.Now;
                SessionType type = startSessionVM.Type;

                if(elpasedTime is not null && startAt is not null)
                {
                    // session can't start in the future
                    if(startAt.Value > now)
                    {
                        return new ValidationResult("Recorded Session Cannot start in the future");
                    }
                    // Recorded session can't run in the future
                    if(startAt.Value + elpasedTime.Value > now)
                    {
                        return new ValidationResult("Recorded Session Cannot end in the future");
                    }
                }
                else
                {
                    if (type == SessionType.Recorded)
                    {
                        return new ValidationResult("Recorded Session Fields are Required");
                    }
                }

                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid Usage");
        }
    }
}
