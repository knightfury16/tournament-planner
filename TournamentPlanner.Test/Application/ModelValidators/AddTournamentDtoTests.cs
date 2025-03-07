using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Test.Application.ModelValidators
{
    public class AddTournamentDtoTests
    {
        [Fact]
        public void AddTournamentDto_ValidData_ShouldPassValidation()
        {
            //Arrange
            var tournamentDto = GetAValidTournamentDto();
            //Act
            var validationResult = ValidateModel(tournamentDto);
            //Assert
            Assert.Empty(validationResult);
        }

        [Theory]
        [InlineData("", "The Name field is required.")]
        [InlineData("Test", "Name should at least 5 character long")]
        [InlineData(
            "ThisNameIsTooLongThisNameIsTooLongThisNameIsTooLongThisNameIsTooLongThisNameIsTooLongThisNameIsTooLong",
            "Name should at most 100 charecter long"
        )]
        public void AddTournamentDto_InvalidName_ShouldFailValidation(
            string name,
            string expectedErrorMessage
        )
        {
            // Arrange
            var dto = GetAValidTournamentDto();
            dto.Name = name;

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Contains(
                validationResults,
                vr => vr.ErrorMessage!.Contains(expectedErrorMessage)
            );
        }

        [Fact]
        public void AddTournamentDto_StartDateAfterEndDate_ShouldFailValidation()
        {
            // Arrange
            var dto = GetAValidTournamentDto();
            dto.EndDate = dto.StartDate.AddDays(-1);
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Contains(
                validationResults,
                vr =>
                    vr.ErrorMessage!.Contains(
                        "The start date must be earlier than or equal to the end date"
                    )
            );
        }



        [Theory]
        [InlineData(0)]
        [InlineData(124)]
        public void AddTournamentDto_InvalidKnockOutStartNumber_ShouldFailValidation(
            int knockOutStartNumber
        )
        {
            // Arrange
            var dto = GetAValidTournamentDto();
            dto.KnockOutStartNumber = knockOutStartNumber;
            // Act
            var validationResults = ValidateModel(dto);
            // Assert
            Assert.Contains(
                validationResults,
                vr => vr.ErrorMessage!.Contains("Knockout start number should be between 2 to 64")
            );
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(12)]
        [InlineData(24)]
        [InlineData(48)]
        public void AddTournamentDto_NonPowerOfTwoKnockOutStartNumber_ShouldFailValidation(
            int knockOutStartNumber
        )
        {
            // Arrange
            var dto = GetAValidTournamentDto();
            dto.KnockOutStartNumber = knockOutStartNumber;
            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.Contains(
                validationResults,
                vr => vr.ErrorMessage!.Contains("KnockOutStartNumber must be a power of two")
            );
        }

        private AddTournamentDto GetAValidTournamentDto()
        {
            return new AddTournamentDto
            {
                Name = "Valid Tournament Name",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                GameType = new GameTypeDto { Name = GameTypeSupported.Chess.ToString() },
                Status = TournamentStatus.Draft,
                RegistrationLastDate = DateTime.Now,
                MaxParticipant = 48, //changed for adding the new feasibility validator
                Venue = "Test Venue",
                RegistrationFee = 10.00m,
                MinimumAgeOfRegistration = 18,
                WinnerPerGroup = 2,
                KnockOutStartNumber = 16,
                ParticipantResolutionStrategy = ResolutionStrategy.StatBased,
                TournamentType = TournamentType.GroupStage,
            };
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResult = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResult, true);
            return validationResult;
        }
    }
}

