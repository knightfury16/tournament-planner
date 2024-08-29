using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.Common.Attributes;

namespace TournamentPlanner.Test.Application.Common.Attributes
{
    public class PowerOfTwoAttributeTests
    {
        [Fact]
        public void Validate_PowerOfTwo_Should_Validate()
        {
            //arrange
            var attribute = new PowerOfTwoAttribute();
            var testObject = new { DisplayName = "TestProperty" };
            var validationContext = new ValidationContext(testObject);
            //act
            var result = attribute.GetValidationResult(8, validationContext);
            //assert
            Assert.Equal(ValidationResult.Success, result);
            Assert.Null(result?.ErrorMessage);
        }

        [Fact]
        public void Invalid_PowerOfTwo_Should_Fail()
        {
            //arrange
            var attribute = new PowerOfTwoAttribute();
            var testObject = new { DisplayName = "TestProperty" };
            var validationContext = new ValidationContext(testObject);
            //act
            var result = attribute.GetValidationResult(7, validationContext);
            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Contains($"{validationContext.DisplayName} must be a power of two", result.ErrorMessage);
        }

        [Fact]
        public void Invalid_Data_Type_Should_Fail()
        {
            //arrange
            var attribute = new PowerOfTwoAttribute();
            var testObject = new { DisplayName = "TestProperty" };
            var validationContext = new ValidationContext(testObject);
            //act
            var result = attribute.GetValidationResult(7.8, validationContext);
            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Contains($"The value is not a valid Integer", result.ErrorMessage);
        }


    }
}