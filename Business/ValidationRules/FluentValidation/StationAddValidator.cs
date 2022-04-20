using FluentValidation;
using static Core.DTOs.Station;

namespace Business.ValidationRules.FluentValidation
{
    public class StationAddValidator : AbstractValidator<StationAdd>
    {
        public StationAddValidator()
        {
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Location).IsInEnum();
            RuleFor(x => x.ProductionLine).NotEmpty();
        }
    }
}
