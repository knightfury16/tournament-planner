using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.Common.Attributes;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Enum;

public class FeasibilityValidatorAttributeTests
{
    // Test for missing properties
    [Fact]
    public void Validate_MissingProperties_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute(
            "MissingProperty",
            "MaxParticipant",
            "KnockOutStartNumber"
        );
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = 16,
            KnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Unkown property", result.ErrorMessage);
    }

    // Test for null TournamentType
    [Fact]
    public void Validate_NullTournamentType_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = null,
            MaxParticipant = 16,
            KnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Tournament Type of the Tournament is not given", result.ErrorMessage);
    }

    // Test for null MaxParticipant
    [Fact]
    public void Validate_NullMaxParticipant_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = null,
            KnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("MaxParticipant or KnockoutStartNumber is not given", result.ErrorMessage);
    }

    // Test for null KnockOutStartNumber
    [Fact]
    public void Validate_NullKnockOutStartNumber_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = 16,
            KnockOutStartNumber = null,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("MaxParticipant or KnockoutStartNumber is not given", result.ErrorMessage);
    }

    // Test for Knockout tournament with valid participant count
    [Fact]
    public void Validate_KnockoutTournament_ValidParticipantCount_Succeeds()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = Utility.KnocoutMatchTypeMaxParticipant,
            KnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    // Test for Knockout tournament with excessive participant count
    [Fact]
    public void Validate_KnockoutTournament_ExcessiveParticipantCount_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = Utility.KnocoutMatchTypeMaxParticipant + 1,
            KnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Knockout match type max participants count", result.ErrorMessage);
    }

    // Test for GroupStage tournament with valid player per group count
    [Fact]
    public void Validate_GroupStageTournament_ValidPPG_Succeeds()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        // Setting values to ensure PPG will be less than or equal to Utility.GroupMatchTypePPG
        int knockoutStartNumber = 8;
        int maxParticipants = knockoutStartNumber * Utility.GroupMatchTypePPG / 2;

        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.GroupStage,
            MaxParticipant = maxParticipants,
            KnockOutStartNumber = knockoutStartNumber,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    // Test for GroupStage tournament with excessive player per group count
    [Fact]
    public void Validate_GroupStageTournament_ExcessivePPG_ReturnsError()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        // Setting values to ensure PPG will exceed Utility.GroupMatchTypePPG
        int knockoutStartNumber = 4;
        int maxParticipants = knockoutStartNumber * (Utility.GroupMatchTypePPG + 1) / 2;

        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.GroupStage,
            MaxParticipant = maxParticipants,
            KnockOutStartNumber = knockoutStartNumber,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Player per group max count", result.ErrorMessage);
    }

    // Test for custom property names
    [Fact]
    public void Validate_CustomPropertyNames_Succeeds()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute(
            "CustomTournamentType",
            "CustomMaxParticipant",
            "CustomKnockOutStartNumber"
        );
        var testTournament = new CustomTournament
        {
            CustomTournamentType = TournamentType.Knockout,
            CustomMaxParticipant = 16,
            CustomKnockOutStartNumber = 8,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    // Test for edge case with minimum participants
    [Fact]
    public void Validate_MinimumParticipants_Succeeds()
    {
        // Arrange
        var validator = new FeasibilityValidatorAttribute();
        var testTournament = new TestTournament
        {
            TournamentType = TournamentType.Knockout,
            MaxParticipant = 2, // Minimum possible for a tournament
            KnockOutStartNumber = 2,
        };
        var validationContext = new ValidationContext(testTournament);

        // Act
        var result = validator.GetValidationResult(null, validationContext);

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    // Helper classes for testing
    private class TestTournament
    {
        public TournamentType? TournamentType { get; set; }
        public int? MaxParticipant { get; set; }
        public int? KnockOutStartNumber { get; set; }
    }

    private class CustomTournament
    {
        public TournamentType? CustomTournamentType { get; set; }
        public int? CustomMaxParticipant { get; set; }
        public int? CustomKnockOutStartNumber { get; set; }
    }
}
