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
            RuleFor(x => x.ProductionDate).Must(CheckDateIfNotValid).WithMessage(Messages.InValidDate);
            RuleFor(x => x.BottleType).NotEmpty();
            RuleFor(x => x.BottleType).IsInEnum();
        }

        private bool CheckDateIfNotValid(string value)
        {
            var format = DateTimeOffset.TryParseExact(value, BottleConstants.ProductionDateFormat,
                null, System.Globalization.DateTimeStyles.None, out var date);
            return format && date.Date <= DateTimeOffset.UtcNow.Date;
        }
    }
}
