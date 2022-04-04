using Core.Constants;
using FluentValidation;
using static Core.DTOs.User;

namespace Business.ValidationRules.FluentValidation
{
    public class PanelUserUpdateValidator : AbstractValidator<PanelUserUpdateRequest>
    {
        public PanelUserUpdateValidator()
        {

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(UserConstants.MaxNameLength);

            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Surname).MaximumLength(UserConstants.MaxNameLength);

            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.UserType).NotEmpty();
            RuleFor(x => x.UserType).IsInEnum();
        }
    }
}
