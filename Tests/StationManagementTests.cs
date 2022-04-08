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
        [InlineData(1)]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Location(int location)
        {
            stationAddDto.Location = location;
            _addValidator.TestValidate(stationAddDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(null)]
        public void StationAddValidator_Should_Have_Validation_Error_For_ProductionLine_Null(string productin_line)
        {
            stationAddDto.ProductionLine = productin_line;
            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Fact]
        public void StationAddValidator_Should_Have_Validation_Error_For_ProductionLine_Empty()
        {
            stationAddDto.ProductionLine = string.Empty;
            _addValidator.TestValidate(stationAddDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
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
        [InlineData(1, 4, "line_1", 1)]
        public void StationAddValidator_Should_Not_Have_Validation_Errors_Or_Not_Throw_Exception(
            int userId, int userType, string production_line, int location)
        {

            user.Type = userType;

            stationAddDto.PanelUserId = userId;
            stationAddDto.ProductionLine = production_line;
            stationAddDto.Location = location;

            _iPaneUserService.Setup(x => x.GetById(stationAddDto.PanelUserId)).Returns(user);

            _stationRepositoryMock.Setup(x=>x.GetByPanelUserId(userId)).Returns((Station)null);

            _iStationService.Invoking(x => x.Add(stationAddDto))
                            .Should().NotThrow<CustomException>();

            _addValidator.TestValidate(stationAddDto).ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_Location()
        {
            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(1)]
        public void StationUpdateValidator_Should_Not_Have_Validation_Error_For_Location(int location)
        {
            stationUpdateDto.Location = location;
            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveValidationErrorFor(x => x.Location);
        }

        [Theory]
        [InlineData(null)]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_ProductionLine_Null(string productin_line)
        {
            stationUpdateDto.ProductionLine = productin_line;
            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
        }

        [Fact]
        public void StationUpdateValidator_Should_Have_Validation_Error_For_ProductionLine_Empty()
        {
            stationUpdateDto.ProductionLine = string.Empty;
            _updateValidator.TestValidate(stationUpdateDto).ShouldHaveValidationErrorFor(x => x.ProductionLine);
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
        [InlineData(1, 4, "line_1", 1)]
        public void StationService_Update_Should_Not_Have_Validation_Errors_Or_Not_Throw_Exception(
            int userId, int userType, string production_line, int location)
        {

            user.Type = userType;

            stationUpdateDto.PanelUserId = userId;
            stationUpdateDto.ProductionLine = production_line;
            stationUpdateDto.Location = location;

            _iPaneUserService.Setup(x => x.GetById(stationUpdateDto.PanelUserId)).Returns(user);

            _stationRepositoryMock.Setup(x => x.GetByPanelUserId(userId)).Returns((Station)null);

            _iStationService.Invoking(x => x.Update(stationUpdateDto))
                            .Should().NotThrow<CustomException>();

            _updateValidator.TestValidate(stationUpdateDto).ShouldNotHaveAnyValidationErrors();
        }
    }
}
