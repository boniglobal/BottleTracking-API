using Core.Constants;
using FluentValidation;
using static Core.DTOs.Bottle;

namespace Business.ValidationRules.FluentValidation
{
    public class BottleAddValidator : AbstractValidator<BottleAdd>
    {
        public BottleAddValidator()
        {
            RuleFor(x => x.ProductionDate).NotEmpty();
            RuleFor(x => x.ProductionDate).Must(CheckDateIfNotValid).WithMessage(Messages.InValidDate);
            RuleFor(x => x.BottleType).NotEmpty();
            RuleFor(x => x.BottleType).IsInEnum();
            RuleFor(x => x.RefillCount).GreaterThanOrEqualTo(0).When(x => x.RefillCount is not null);
        }

        private bool CheckDateIfNotValid(string value)
        {
            var format =  DateTimeOffset.TryParseExact(value, BottleConstants.ProductionDateFormat,
                null, System.Globalization.DateTimeStyles.None, out var date);
            return format && date.Date <= DateTimeOffset.UtcNow.Date;
        }
    }
}
