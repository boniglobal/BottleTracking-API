using Business.Services;
using Business.Utilities;
using Business.ValidationRules.FluentValidation;
using Core.Constants;
using Data.Abstract;
using Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Xunit;
using static Core.Constants.StationConstants;
using static Core.DTOs.Station;

namespace Tests
{
    public class StationTests
    {

        private readonly StationAdd stationAddDto;
        private readonly StationUpdate stationUpdateDto;

        private readonly Mock<IStationRepository> _stationRepositoryMock;

        private readonly IStationService _iStationService;
        private readonly Mock<IPaneUserService> _iPaneUserService;
        private readonly StationAddValidator _addValidator;
        private readonly StationUpdateValidator _updateValidator;

        private readonly PanelUser user;
        public StationTests()
        {
            _stationRepositoryMock = new Mock<IStationRepository>();

            _iPaneUserService = new Mock<IPaneUserService>();
            _iStationService = new StationService(_stationRepositoryMock.Object, _iPaneUserService.Object);

            stationAddDto = new StationAdd();
            stationUpdateDto = new StationUpdate();

            _addValidator = new StationAddValidator();
            _updateValidator = new StationUpdateValidator();


            user = new PanelUser();
        }

        [Fact]
        public void StationAddValidator_Should_Have_Validation_Error_For_Location()
        {
            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(3)]
        public void StationAddValidator_Should_Have_Validation_Error_For_Location_Not_In_Enum(int location)
        {
            stationAddDto.Location = (Locations)location;
            _addValidator.TestValidate(stationAddDto)
                         .ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(1)]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Location(int location)
        {
            stationAddDto.Location = (Locations)location;
            _addValidator.TestValidate(stationAddDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(2)]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Location1(int location)
        {
            stationAddDto.Location = (Locations)location;
            _addValidator.TestValidate(stationAddDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(100)]
        public void StationAddValidator_Should_Have_Validation_Error_For_ProductionLine(int production_line)
        {
            stationAddDto.ProductionLine = production_line;
            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(1)]
        public void StationAddValidator_Should_Not_Have_Validation_Error_For_ProductionLine(int production_line)
        {
            stationAddDto.ProductionLine = production_line;
            _addValidator.TestValidate(stationAddDto).ShouldNotHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(1)]
        public void StationService_Add_Should_Throw_Exception_For_PanelUser_AssignUserMoreThanOneStation(int userId)
        {
            stationAddDto.PanelUserId = userId;
            _stationRepositoryMock.Setup(x=>x.GetByPanelUserId(stationAddDto.PanelUserId))
                                  .Returns(new Station { PanelUserId = userId});

            _iStationService.Invoking(x=>x.Add(stationAddDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.AssignUserMoreThanOneStation);
        }
        [Theory]
        [InlineData(1)]
        public void StationAddValidator_Should_Throw_Exception_Error_For_PanelUser_UserNotFound(int userId)
        {
            stationAddDto.PanelUserId = userId;
            _iPaneUserService.Setup(x=>x.GetById(userId)).Returns((PanelUser)null);

            _iStationService.Invoking(x=>x.Add(stationAddDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.UserNotFound);
        }

        [Theory]
        [InlineData(1)]
        public void StationAddValidator_Should_Throw_Exception_For_PanelUser_UserType(int userId)
        {

            user.Type = 1;

            stationAddDto.PanelUserId = userId;

            _iPaneUserService.Setup(x=>x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _iStationService.Invoking(x=>x.Add(stationAddDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.AssignDifferentUserType);
        }

        [Theory]
        [InlineData(1, 4, 1, 1)]
        public void StationAddValidator_Should_Not_Have_Validation_Errors_Or_Not_Throw_Exception(
            int userId, int userType, int production_line, int location)
        {

            user.Type = userType;

            stationAddDto.PanelUserId = userId;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.Location = (Locations)location;

            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _stationRepositoryMock.Setup(x=>x.GetByPanelUserId(userId)).Returns((Station)null);

            _iStationService.Invoking(x => x.Add(stationAddDto))
                            .Should().NotThrow<CustomException>();

            _addValidator.TestValidate(stationAddDto).ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_Location_Empty()
        {
            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(3)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_Location_Not_In_Enum(int location)
        {
            stationUpdateDto.Location = (Locations)location;
            _updateValidator.TestValidate(stationUpdateDto)
                         .ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(1)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Error_For_Location_In_Enum(int location)
        {
            stationUpdateDto.Location = (Locations)location;
            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(2)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Error_For_Location_In_Enum1(int location)
        {
            stationUpdateDto.Location = (Locations)location;
            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(0)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_ProductionLine(int production_line)
        {
            stationUpdateDto.ProductionLine = production_line;
            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(99)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Error_For_ProductionLine(int production_line)
        {
            stationUpdateDto.ProductionLine = production_line;
            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        public void StationService_Update_Should_Throw_Exception_For_PanelUser_AssignUserMoreThanOneStation(
            int updateStationId, int exsitingStationId, int userId)
        {
            stationUpdateDto.StationId = updateStationId;
            stationUpdateDto.PanelUserId = userId;
            _stationRepositoryMock.Setup(x => x.GetByPanelUserId(stationUpdateDto.PanelUserId))
                                  .Returns(new Station { Id = exsitingStationId, PanelUserId = userId });

            _iStationService.Invoking(x => x.Update(stationUpdateDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.AssignUserMoreThanOneStation);
        }
        [Theory]
        [InlineData(1)]
        public void StationUpdateValidator_Should_Throw_Exception_Error_For_PanelUser_UserNotFound(int userId)
        {
            stationUpdateDto.PanelUserId = userId;
            _iPaneUserService.Setup(x => x.GetById(userId)).Returns((PanelUser)null);

            _iStationService.Invoking(x => x.Update(stationUpdateDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.UserNotFound);
        }

        [Theory]
        [InlineData(1, 1)]
        public void StationService_Update_Should_Throw_Exception_For_PanelUser_UserType(int userId, int UserType)
        {

            user.Type = UserType;

            stationUpdateDto.PanelUserId = userId;

            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _iStationService.Invoking(x => x.Update(stationUpdateDto))
                            .Should().Throw<CustomException>()
                            .WithMessage(Messages.AssignDifferentUserType);
        }

        [Theory]
        [InlineData(1, 4, 1, 1)]
        public void StationService_Update_Should_Not_Have_Validation_Errors_Or_Not_Throw_Exception(
            int userId, int userType, int production_line, int location)
        {

            user.Type = userType;

            stationUpdateDto.PanelUserId = userId;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.Location = (Locations)location;

            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _stationRepositoryMock.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _iStationService.Invoking(x => x.Update(stationUpdateDto))
                            .Should().NotThrow<CustomException>();

            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveAnyValidationErrors();
        }
    }
}
