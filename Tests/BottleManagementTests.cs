using Business.ValidationRules.FluentValidation;
using Core.Constants;
using FluentValidation.TestHelper;
using Xunit;
using static Core.DTOs.Bottle;

namespace Tests
{
    public class BottleManagementTests
    {
        private BottleAdd _addDto;
        private BottleUpdate _updateDto;

        private readonly BottleAddValidator _bottleAddValidator;
        private readonly BottleUpdateValidator _bottleUpdateValidator;
        public BottleManagementTests()
        {
            _addDto = new();
            _updateDto = new();

            _bottleAddValidator = new();
            _bottleUpdateValidator = new();
        }

        [Fact]
        public void BottleAddValidator_Should_Have_Validation_Error_For_ProductionDate_Null()
        {
            _addDto.ProductionDate = null;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage("'Production Date' must not be empty.");
        }

        [Fact]
        public void BottleAddValidator_Should_Have_Validation_Error_For_ProductionDate_Empty()
        {
            _addDto.ProductionDate = string.Empty;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage("'Production Date' must not be empty.");
        }

        [Theory]
        [InlineData("01.2022")]
        public void BottleAddValidator_Should_Have_Validation_Error_For_ProductionDate_InvalidDateFormat(string date)
        {

            _addDto.ProductionDate = date;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage(Messages.InValidDateFormat);
        }

        [Theory]
        [InlineData("14/2022")]
        public void BottleAddValidator_Should_Have_Validation_Error_For_ProductionDate_InvalidDateFormat1(string date)
        {

            _addDto.ProductionDate = date;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage(Messages.InValidDateFormat);
        }

        [Theory]
        [InlineData("01/2022")]
        public void BottleAddValidator_Should_Not_Have_Validation_Error_For_ProductionDate(string dateValue)
        {

            _addDto.ProductionDate = dateValue;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void BottleUpdateValidator_Should_Have_Validation_Error_For_ProductionDate_Null()
        {
            _updateDto.ProductionDate = null;
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage("'Production Date' must not be empty.");
        }

        [Fact]
        public void BottleUpdateValidator_Should_Have_Validation_Error_For_ProductionDate_Empty()
        {
            _updateDto.ProductionDate = string.Empty;
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage("'Production Date' must not be empty.");
        }

        [Theory]
        [InlineData("01.2022")]
        public void BottleUpdateValidator_Should_Have_Validation_Error_For_ProductionDate_InvalidDateFormat(string dateValue)
        {

            _updateDto.ProductionDate = dateValue;
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate)
                               .WithErrorMessage(Messages.InValidDateFormat);
        }

        [Theory]
        [InlineData("01/2022")]
        public void BottleUpdateValidator_Should_Not_Have_Validation_Error_For_ProductionDate_InvalidDateFormat(string dateValue)
        {

            _updateDto.ProductionDate = dateValue;
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldNotHaveValidationErrorFor(x => x.ProductionDate);
        }
    }
}
