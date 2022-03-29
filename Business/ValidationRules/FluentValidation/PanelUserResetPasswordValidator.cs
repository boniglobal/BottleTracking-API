using Core.Constants;
using FluentValidation;
using static Core.DTOs.User;

namespace Business.ValidationRules.FluentValidation
{
    public class PanelUserResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public PanelUserResetPasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(UserConstants.MinPasswordLength);
            RuleFor(x => x.Password).Matches(@"[A-Za-z]+").WithMessage(Messages.PasswordWithoutLetter);
            RuleFor(x => x.Password).Matches(@"[0-9]+").WithMessage(Messages.PasswordWithoutNumber);
        }
    }
}
