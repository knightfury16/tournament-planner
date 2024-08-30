using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.Common.Attributes;

namespace TournamentPlanner.Test.Application.Common.Attributes
{
    public class FutureAttributeTests
    {
        // Validates a future date successfully
        [Fact]
        public void validates_future_date_successfully()
        {
            // Arrange
            var futureAttribute = new FutureAttribute();
            var futureDate = DateTime.Now.AddDays(1);
            var validationContext = new ValidationContext(new { DisplayName = "Start Date" });

            // Act
            var result = futureAttribute.GetValidationResult(futureDate, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        // Handles null date input gracefully
        [Fact]
        public void handles_null_date_input_gracefully()
        {
            // Arrange
            var futureAttribute = new FutureAttribute();
            DateTime? nullDate = null;
            var validationContext = new ValidationContext(new { DisplayName = "Start Date" });

            // Act
            var result = futureAttribute.GetValidationResult(nullDate, validationContext);

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"{validationContext.DisplayName}: unkmowm property date.", result.ErrorMessage);
        }

        // Returns error for past dates
        [Fact]
        public void returns_error_for_past_dates()
        {
            // Arrange
            var futureAttribute = new FutureAttribute();
            var pastDate = DateTime.Now.AddDays(-1);
            var validationContext = new ValidationContext(new { DisplayName = "Start Date" });

            // Act
            var result = futureAttribute.GetValidationResult(pastDate, validationContext);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid start date. Please select a date which is in the future.", result.ErrorMessage);
        }

    }
}