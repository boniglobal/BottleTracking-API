using Business.ValidationRules.FluentValidation;
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
        public void BottleAddValidator_Should_Have_Validation_Error_For_ProductionDate()
        {
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldHaveValidationErrorFor(x=>x.ProductionDate);
        }
        [Fact]
        public void BottleAddValidator_Should_Not_Have_Validation_Error_For_ProductionDate()
        {

            _addDto.ProductionDate = System.DateTimeOffset.UtcNow;
            _bottleAddValidator.TestValidate(_addDto)
                               .ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void BottleUpdateValidator_Should_Have_Validation_Error_For_ProductionDate()
        {
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldHaveValidationErrorFor(x => x.ProductionDate);
        }
        [Fact]
        public void BottleUpdateValidator_Should_Not_Have_Validation_Error_For_ProductionDate()
        {

            _updateDto.ProductionDate = System.DateTimeOffset.UtcNow;
            _bottleUpdateValidator.TestValidate(_updateDto)
                               .ShouldNotHaveAnyValidationErrors();
        }
    }
}
