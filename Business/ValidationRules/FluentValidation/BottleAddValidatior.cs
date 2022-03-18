using FluentValidation;
using static Core.DTOs.Bottle;

namespace Business.ValidationRules.FluentValidation
{
    public class BottleAddValidator : AbstractValidator<BottleAdd>
    {
        public BottleAddValidator()
        {
            RuleFor(x=>x.ProductionDate).NotEmpty();
        }
    }
}
