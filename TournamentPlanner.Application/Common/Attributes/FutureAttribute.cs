using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.Common.Attributes;
public class FutureAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        DateTime? startDate = value as DateTime?;

        if (startDate == null)
        {
            return new ValidationResult($"{validationContext.DisplayName}: unkmowm property date.");
        }
        if (startDate.HasValue && startDate.Value < DateTime.Now)
        {
            return new ValidationResult("Invalid start date. Please select a date which is in the future.");
        }
        return ValidationResult.Success;
    }

}