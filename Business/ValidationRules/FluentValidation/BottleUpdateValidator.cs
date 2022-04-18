using Core.Constants;
using FluentValidation;
using static Core.DTOs.Bottle;

namespace Business.ValidationRules.FluentValidation
{
    public class BottleUpdateValidator : AbstractValidator<BottleUpdate>
    {
        public BottleUpdateValidator()
        {
            RuleFor(x => x.ProductionDate).NotEmpty();
            RuleFor(x => x.ProductionDate).Must(CheckDateIfNotValid).WithMessage(Messages.InValidDateFormat);
            RuleFor(x => x.BottleType).NotEmpty();
            RuleFor(x => x.BottleType).IsInEnum();
        }

        private bool CheckDateIfNotValid(string value)
        {
            return DateTimeOffset.TryParseExact(value, BottleConstants.ProductionDateFormat,
                null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}
