using Business.Services;
using Business.ValidationRules.FluentValidation;
using Core.Constants;
using Entities;
using FluentValidation.TestHelper;
using Moq;
using Xunit;
using static Core.DTOs.Station;

namespace Tests
{
    public class StationTests
    {

        private StationAdd stationAddDto;
        private StationUpdate stationUpdateDto;

        private Mock<IStationService> _iStationService;
        private Mock<IPaneUserService> _iPaneUserService;
        private readonly StationAddValidator _addValidator;
        private readonly StationUpdateValidator _updateValidator;

        private Station station;
        private PanelUser user;
        public StationTests()
        {
            _iStationService = new Mock<IStationService>();
            _iPaneUserService = new Mock<IPaneUserService>();

            stationAddDto = new StationAdd();
            stationUpdateDto = new StationUpdate();

            _addValidator = new StationAddValidator(_iStationService.Object, _iPaneUserService.Object);
            _updateValidator = new StationUpdateValidator(_iStationService.Object, _iPaneUserService.Object);

            station = new Station();
            user = new PanelUser();
        }

        [Theory]
        [InlineData("production_line", 1)]
        public void StationAddValidator_Should_Have_Validation_Error_For_Location(string production_line, int userId)
        {
            stationAddDto.ProductionLine = production_line;
            stationAddDto.PanelUserId = userId;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(1, 1)]
        public void StationAddValidator_Should_Have_Validation_Error_For_ProductionLine(int location, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.PanelUserId = userId;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationAddValidator_Should_Have_Validation_Error_For_PanelUser(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.PanelUserId = userId;
            _iStationService.Setup(x => x.GetByPanelUserId(stationAddDto.PanelUserId)).Returns(station);

            _addValidator.TestValidate(stationAddDto)
                         .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                         .WithErrorMessage(Messages.AssignUserMoreThanOneStation);
        }
        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationAddValidator_Should_Have_Validation_Error_For_PanelUser_User_Not_Found(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.PanelUserId = userId;
            _iStationService.Setup(x => x.GetByPanelUserId(stationAddDto.PanelUserId)).Returns(station);
            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns((PanelUser)null);

            _addValidator.TestValidate(stationAddDto)
                         .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                         .WithErrorMessage(Messages.UserNotFound);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationAddValidator_Should_Have_Validation_Error_For_PanelUser_UserType(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.PanelUserId = userId;

            user.Type = 1;

            _iStationService.Setup(x => x.GetByPanelUserId(stationAddDto.PanelUserId)).Returns(station);
            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _addValidator.TestValidate(stationAddDto)
                         .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                         .WithErrorMessage(Messages.AssignDifferentUserType);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationAddValidator_Should_Not_Have_Validation_Errors(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.PanelUserId = userId;

            user.Type = 4;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _addValidator.TestValidate(stationAddDto).ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, "", 1)]
        public void StationAddValidator_Should_Have_Validation_Errors(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _addValidator.TestValidate(stationAddDto).ShouldHaveAnyValidationError();
        }

        [Theory]
        [InlineData(1, null, 1)]
        public void StationAddValidator_Should_Have_Validation_Errors_1(int location, string production_line, int userId)
        {
            stationAddDto.Location = location;
            stationAddDto.ProductionLine = production_line;
            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _addValidator.TestValidate(stationAddDto).ShouldHaveAnyValidationError();
        }

        [Theory]
        [InlineData("production_line", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_Location(string production_line, int userId)
        {
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(userId)).Returns(user);

            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(1, 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_ProductionLine(int location, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.PanelUserId = userId;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(userId)).Returns(user);

            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_PanelUser(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;
            _iStationService.Setup(x => x.GetByPanelUserId(stationUpdateDto.PanelUserId)).Returns(station);

            _updateValidator.TestValidate(stationUpdateDto)
                            .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                            .WithErrorMessage(Messages.AssignUserMoreThanOneStation);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_PanelUser_User_Not_Found(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;

            _iStationService.Setup(x => x.GetByPanelUserId(stationUpdateDto.PanelUserId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns((PanelUser)null);

            _updateValidator.TestValidate(stationUpdateDto)
                            .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                            .WithErrorMessage(Messages.UserNotFound);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_PanelUser_AssignDifferentUserType(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;

            user.Type = 1;
            _iStationService.Setup(x => x.GetByPanelUserId(stationUpdateDto.PanelUserId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _updateValidator.TestValidate(stationUpdateDto)
                            .ShouldHaveValidationErrorFor(x => x.PanelUserId)
                            .WithErrorMessage(Messages.AssignDifferentUserType);
        }

        [Theory]
        [InlineData(1, 1, "production_line", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_PanelUser_1(int id, int location, string production_line, int userId)
        {
            stationUpdateDto.StationId = id;
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;

            station.Id = id;
            station.PanelUserId = userId;
            _iStationService.Setup(x => x.GetByPanelUserId(stationUpdateDto.PanelUserId)).Returns(station);

            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.PanelUserId);
        }

        [Theory]
        [InlineData(1, "production_line", 1)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Errors(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;
            user.Type = 4;
            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);
            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, 1, "production_line", 2)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Errors_1(int id, int location, string production_line, int userId)
        {
            stationUpdateDto.StationId = id;
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.PanelUserId = userId;

            station.Id = userId;
            user.Type = 4;

            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns(station);
            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, "", 1)]
        public void StationUpdateValidator_Should_Have_Validation_Errors(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveAnyValidationError();
        }

        [Theory]
        [InlineData(1, null, 1)]
        public void StationUpdateValidator_Should_Have_Validation_Errors_1(int location, string production_line, int userId)
        {
            stationUpdateDto.Location = location;
            stationUpdateDto.ProductionLine = production_line;
            _iStationService.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveAnyValidationError();
        }
    }
}
