using Business.Services;
using Core.Constants;
using Entities;
using FluentValidation;
using static Core.DTOs.Station;

namespace Business.ValidationRules.FluentValidation
{
    public class StationAddValidator : AbstractValidator<StationAdd>
    {
        private readonly IStationService _stationService;
        private readonly IPaneUserService _paneUserService;
        private PanelUser user;
        public StationAddValidator(
            IStationService stationService,
            IPaneUserService paneUserService)
        {
            _stationService = stationService;
            _paneUserService = paneUserService;

            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.ProductionLine).NotEmpty();
            RuleFor(x => x.PanelUserId).Must(CheckUserIfAssignedToAnotherStation).WithMessage(Messages.AssignUserMoreThanOneStation);
            RuleFor(x => x.PanelUserId).Must(CheckUserIfNotFound).WithMessage(Messages.UserNotFound);
            RuleFor(x=>x.PanelUserId).Must(CheckUserIfNotKiosk).When(x => user != null).WithMessage(Messages.AssignDifferentUserType);
        }

        private bool CheckUserIfAssignedToAnotherStation(int id)
        {
            var station = _stationService.GetByPanelUserId(id);
            return station == null;
        }

        private bool CheckUserIfNotFound(int panelUserId)
        {
            var panelUser = _paneUserService.GetById(panelUserId);
            user = panelUser;
            return panelUser != null;
        }

        private bool CheckUserIfNotKiosk(int panelUserId)
        {
            return user.Type == (int)UserConstants.Types.Kiosk;
        }
    }
}
