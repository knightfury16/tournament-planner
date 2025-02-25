using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FeasibilityValidatorAttribute : ValidationAttribute
    {
        private const string DefaultTournamentTypePropName = "TournamentType";
        private const string DefaultMaxParticipantPropName = "MaxParticipant";
        private const string DefaultKnockOutStartNumber = "KnockOutStartNumber";
        private readonly string _tournamentTypePropName;
        private readonly string _maxParticipantPropName;
        private readonly string _knockoutstartnumberPropName;

        public FeasibilityValidatorAttribute(
            string tournamentTypePropName = DefaultTournamentTypePropName,
            string maxparticipantPropName = DefaultMaxParticipantPropName,
            string knockoutstartnumberPropName = DefaultKnockOutStartNumber
        )
        {
            _tournamentTypePropName = tournamentTypePropName;
            _maxParticipantPropName = maxparticipantPropName;
            _knockoutstartnumberPropName = knockoutstartnumberPropName;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            Console.WriteLine(
                "Validation context performing on instance of : ",
                validationContext.DisplayName
            );
            var tournamentTypeProp = validationContext.ObjectType.GetProperty(
                _tournamentTypePropName
            );
            var maxParticipantProp = validationContext.ObjectType.GetProperty(
                _maxParticipantPropName
            );
            var knockoutStartProp = validationContext.ObjectType.GetProperty(
                _knockoutstartnumberPropName
            );

            if (
                tournamentTypeProp == null
                || maxParticipantProp == null
                || knockoutStartProp == null
            )
            {
                return new ValidationResult(
                    $"Unkown property: {_tournamentTypePropName}, {_maxParticipantPropName}, {_knockoutstartnumberPropName}"
                );
            }
            var tournamentTypeValue =
                tournamentTypeProp.GetValue(validationContext.ObjectInstance) as TournamentType?;
            var maxparticipantValue =
                maxParticipantProp.GetValue(validationContext.ObjectInstance) as int?;
            var knockoutStartNumberValue =
                knockoutStartProp.GetValue(validationContext.ObjectInstance) as int?;

            if (tournamentTypeValue == null)
            {
                return new ValidationResult(
                    "Feasibility Validation Fail: Tournament Type of the Tournament is not given."
                );
            }

            if (!maxparticipantValue.HasValue || !knockoutStartNumberValue.HasValue)
            {
                return new ValidationResult(
                    "Feasibility Validation Fail: MaxParticipant or KnockoutStartNumber is not given."
                );
            }

            if (
                tournamentTypeValue == TournamentType.Knockout
                && maxparticipantValue > Utility.KnocoutMatchTypeMaxParticipant
            )
            {
                return new ValidationResult(
                    "Feasibility Validation Fail: Knockout match type max participants count exceeded."
                );
            }

            if (tournamentTypeValue == TournamentType.GroupStage)
            {
                var ppg = GetPPG(maxparticipantValue, knockoutStartNumberValue);
                if (ppg > Utility.GroupMatchTypePPG)
                {
                    return new ValidationResult(
                        "Feasibility Validation Fail: Player per group max count exceeded."
                    );
                }
            }

            return ValidationResult.Success;
        }

        private int GetPPG(int? maxparticipantValue, int? knockoutStartNumberValue)
        {
            return (int)
                Math.Ceiling(
                    2 * ((double)maxparticipantValue! / (double)knockoutStartNumberValue!)
                ); //I have checked for null before
        }
    }
}
