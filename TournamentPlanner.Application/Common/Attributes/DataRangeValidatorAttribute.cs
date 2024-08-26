using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.Common.Attributes;

public class DataRangeValidatorAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;
    private readonly string _endDatePropertyName;

    public DataRangeValidatorAttribute(string startDatePropertyName, string endDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
        _endDatePropertyName = endDatePropertyName;
    }
    public DataRangeValidatorAttribute()
    {
        _startDatePropertyName = "StartDate";
        _endDatePropertyName = "EndDate";
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        Console.WriteLine("Validation context performing on instance of : ", validationContext.DisplayName);
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        var endDateProperty = validationContext.ObjectType.GetProperty(_endDatePropertyName);

        if (startDateProperty == null || endDateProperty == null)
        {
            return new ValidationResult($"Unkown property: {_startDatePropertyName}, {_endDatePropertyName}");
        }
        var startDateValue = startDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
        var endDateValue = endDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (startDateValue.HasValue && endDateValue.HasValue)
        {
            if (endDateValue < startDateValue)
            {
                return new ValidationResult("The start date must be earlier than or equal to the end date");
            }
        }

        return ValidationResult.Success;
    }
}