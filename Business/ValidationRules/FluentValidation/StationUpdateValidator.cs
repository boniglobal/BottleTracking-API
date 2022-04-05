using FluentValidation;
using static Core.DTOs.Station;

namespace Business.ValidationRules.FluentValidation
{
    public class StationUpdateValidator : AbstractValidator<StationUpdate>
    {
        public StationUpdateValidator()
        {

            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.ProductionLine).NotEmpty();
        }
    }
}
