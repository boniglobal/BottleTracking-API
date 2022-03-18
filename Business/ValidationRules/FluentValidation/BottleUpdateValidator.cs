using FluentValidation;
using static Core.DTOs.Bottle;

namespace Business.ValidationRules.FluentValidation
{
    public class BottleUpdateValidator : AbstractValidator<BottleUpdate>
    {
        public BottleUpdateValidator()
        {
            RuleFor(x=>x.ProductionDate).NotEmpty();
        }
    }
}
