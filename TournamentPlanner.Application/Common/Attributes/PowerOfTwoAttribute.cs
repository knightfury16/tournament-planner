namespace TournamentPlanner.Application.Common.Attributes;

using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public class PowerOfTwoAttribute : ValidationAttribute
{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is null)return ValidationResult.Success;

        if(value is int intValue){
            if(IsPowerOfTwo(intValue)){
                return ValidationResult.Success;
            }
            else{
                return new ValidationResult($"{validationContext.DisplayName} must be a power of two");
            }
        }

        return new ValidationResult("The value is not a valid Integer");
    }


    private bool IsPowerOfTwo(int n)
    {
        if (n == 0) return true;

        while (n != 1)
        {
            if (n % 2 != 0) return false;
            n = n / 2;
        }

        return true;
    }

}

