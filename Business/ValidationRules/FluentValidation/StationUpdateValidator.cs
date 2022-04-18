using Core.Constants;
using FluentValidation;
using static Core.DTOs.Station;

namespace Business.ValidationRules.FluentValidation
{
    public class StationUpdateValidator : AbstractValidator<StationUpdate>
    {
        public StationUpdateValidator()
        {

            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Location).IsInEnum();
            RuleFor(x => x.ProductionLine).InclusiveBetween(StationConstants.MinLineNumber, StationConstants.MaxLineNumber);
        }
    }
}
