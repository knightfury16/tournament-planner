using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.Common.Attributes;

namespace TournamentPlanner.Test.Application.Validators
{
    public class DateRangeValidatorTest
    {
        [Fact]
        public void Validate_StartDate_Before_EndDate()
        {
            //arrange
            var validator = new DateRangeValidatorAttribute();
            var testRange = new { StartDate = new DateTime(2024, 08, 27), EndDate = new DateTime(2024, 08, 28) };
            var validationContext = new ValidationContext(testRange);
            //act
            var result = validator.GetValidationResult(null, validationContext);
            //assert
            Assert.Equal(ValidationResult.Success, result);
            Assert.Null(result?.ErrorMessage);
        }

        [Fact]
        public void Validate_StartDate_After_EndDate()
        {
            //arrange
            var validator = new DateRangeValidatorAttribute();
            var testRange = new { StartDate = new DateTime(2024, 08, 27), EndDate = new DateTime(2024, 08, 26) };
            var validationContext = new ValidationContext(testRange);
            //act
            var result = validator.GetValidationResult(null, validationContext);
            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Equal("The start date must be earlier than or equal to the end date", result.ErrorMessage);
        }

        [Fact]
        public void Validate_StartDate_Equal_EndDate()
        {
            //arrange
            var validator = new DateRangeValidatorAttribute();
            var testRange = new { StartDate = new DateTime(2024, 08, 27), EndDate = new DateTime(2024, 08, 27) };
            var validationContext = new ValidationContext(testRange);
            //act
            var result = validator.GetValidationResult(null, validationContext);
            //assert
            Assert.Equal(ValidationResult.Success, result);
            Assert.Null(result?.ErrorMessage);
        }

        [Fact]
        public void Validate_Wrong_StartDate_Property_Name()
        {
            //arrange
            var validator = new DateRangeValidatorAttribute("StartDae", "EndDate"); // Property name spelled wrong
            var testRange = new { StartDate = new DateTime(2024, 08, 27), EndDate = new DateTime(2024, 08, 27) };
            var validationContext = new ValidationContext(testRange);
            //act
            var result = validator.GetValidationResult(null, validationContext);
            //assert
            Assert.NotNull(result);
            Assert.Equal("Unkown property: StartDae, EndDate", result.ErrorMessage);
        }

        [Fact]
        public void Validate_Wrong_EndDate_Property_Name()
        {
            //arrange
            var validator = new DateRangeValidatorAttribute("StartDate", "EndDae"); // Property name spelled wrong
            var testRange = new { StartDate = new DateTime(2024, 08, 27), EndDate = new DateTime(2024, 08, 27) };
            var validationContext = new ValidationContext(testRange);
            //act
            var result = validator.GetValidationResult(null, validationContext);
            //assert
            Assert.NotNull(result);
            Assert.Equal("Unkown property: StartDate, EndDae", result.ErrorMessage);
        }

    }
}