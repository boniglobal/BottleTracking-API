using Business.Services;
using Core.Constants;
using FluentValidation;
using static Core.DTOs.User;

namespace Business.ValidationRules.FluentValidation
{
    public class PanelUserAddValidator : AbstractValidator<PanelUserAddRequest>
    {
        private readonly IPaneUserService _panelUserService;
        public PanelUserAddValidator(IPaneUserService panelUserService)
        {
            _panelUserService = panelUserService;

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(UserConstants.MaxNameLength);

            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Surname).MaximumLength(UserConstants.MaxNameLength);
            
            RuleFor(x=>x.Email).NotEmpty();
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@((?!\.|\-)[\w\-]+)((\.(\w){2,3})+)$");
            RuleFor(x => x.Email).Must(CheckUserEmailAddress).WithMessage(Messages.NonUniqueEmail);

            RuleFor(x=>x.Password).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(UserConstants.MinPasswordLength);
            RuleFor(x => x.Password).Matches(@"[A-Za-z]+").WithMessage(Messages.PasswordWithoutLetter);
            RuleFor(x => x.Password).Matches(@"[0-9]+").WithMessage(Messages.PasswordWithoutNumber);

            RuleFor(x => x.UserType).NotEmpty();
            RuleFor(x => x.UserType).IsInEnum();
        }

        private bool CheckUserEmailAddress(string email)
        {
            var user = _panelUserService.GetByEmail(email);
            return user == null;
        }
    }
}
