using Business.Services;
using Business.ValidationRules.FluentValidation;
using Core.Constants;
using FluentValidation.TestHelper;
using Moq;
using System;
using Xunit;
using static Core.Constants.UserConstants;
using static Core.DTOs.User;

namespace Tests
{
    public class PanelUserManagementTests
    {
        private readonly Mock<IPaneUserService> _panelUserServiceMock;

        private readonly PanelUserAddValidator _addValidator;
        private readonly PanelUserUpdateValidator _updateValidator;
        private readonly PanelUserResetPasswordValidator _resetPasswordValidator;
        
        private PanelUserAddRequest _addDto;
        private PanelUserUpdateRequest _updateDto;
        private ResetPassword _resetPassword;
        public PanelUserManagementTests()
        {
            _panelUserServiceMock = new Mock<IPaneUserService>();

            _addValidator = new PanelUserAddValidator(_panelUserServiceMock.Object);
            _updateValidator = new PanelUserUpdateValidator(_panelUserServiceMock.Object);
            _resetPasswordValidator = new PanelUserResetPasswordValidator();

            _addDto = new PanelUserAddRequest();
            _updateDto = new PanelUserUpdateRequest();
            _resetPassword = new ResetPassword();
        }

        [Fact]

        public void AddValidator_Should_Have_Validation_Error_For_Name_Empty()
        {
            _addDto.Name = String.Empty;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x=>x.Name);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Name_Null()
        {
            _addDto.Name = null;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Name_Length()
        {
            string name = Guid.NewGuid().ToString();
            _addDto.Name = name;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Uğur")]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Name_Format_1(string name)
        {
            _addDto.Name = name;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Arın")]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Name_Format_2(string name)
        {
            _addDto.Name = name;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Surname_Empty()
        {
            _addDto.Name = String.Empty;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Surname_Null()
        {
            _addDto.Name = null;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Surname_Length()
        {
            string name = Guid.NewGuid().ToString();
            _addDto.Name = name;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Theory]
        [InlineData("Timurcin")]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Surname(string surname)
        {
            _addDto.Surname = surname;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Email_Empty()
        {
            _addDto.Email = String.Empty;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_Email_Null()
        {
            _addDto.Email = null;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("ugurtimurcin@.com")]
        public void AddValidator_Should_Have_Validation_Error_For_Email_Format(string email)
        {
            _addDto.Email = email;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("ugurtimurcin@boniglobal.com")]
        public void AddValidator_Should_Have_Validation_Error_For_Email_NonUniqueEmail(string email)
        {
            _addDto.Email = email;
            _panelUserServiceMock.Setup(x => x.GetByEmail(email)).Returns(new Entities.PanelUser());
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Messages.NonUniqueEmail);
        }

        [Theory]
        [InlineData("ugurtimurcin@boniglobal.com")]
        public void AddValidator_Should_Not_Have_Validation_Error_For_Email(string email)
        {
            _addDto.Email = email;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("123")]
        public void AddValidator_Should_Have_Validation_Error_For_Password(string password)
        {
            _addDto.Password = password;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("1234")]
        public void AddValidator_Should_Have_Validation_Error_For_Password_PasswordWithoutLetter(string password)
        {
            _addDto.Password = password;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Password)
                                               .WithErrorMessage(Messages.PasswordWithoutLetter);
        }

        [Theory]
        [InlineData("ugur")]
        public void AddValidator_Should_Have_Validation_Error_For_Password_PasswordWithoutNumber(string password)
        {
            _addDto.Password = password;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.Password)
                                               .WithErrorMessage(Messages.PasswordWithoutNumber);
        }

        [Theory]
        [InlineData("ugur123")]
        public void AddValidator_Should_Not_Have_Validation_Error(string password)
        {
            _addDto.Password = password;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void AddValidator_Should_Have_Validation_Error_For_UserType()
        {
            _addDto.UserType = (Types)6;
            _addValidator.TestValidate(_addDto).ShouldHaveValidationErrorFor(x => x.UserType);
        }

        [Theory]
        [InlineData(1)]
        public void AddValidator_Should_Not_Have_Validation_Error_For_UserType(int type)
        {
            _addDto.UserType = (Types)type;
            _addValidator.TestValidate(_addDto).ShouldNotHaveValidationErrorFor(x => x.UserType);
        }

        [Theory]
        [InlineData("ugur","timurcin","u@boni.com", "ugur1", 4)]
        public void AddValidator_Should_Not_Have_Any_Validation_Error(string name, string surname, string mail, string password, int type)
        {
            _addDto.Name = name;
            _addDto.Surname = surname;
            _addDto.Email = mail;
            _addDto.UserType = (Types)type;
            _addDto.Password = password;

            _addValidator.TestValidate(_addDto).ShouldNotHaveAnyValidationErrors();
        }

        [Fact]

        public void UpdateValidator_Should_Have_Validation_Error_For_Name_Empty()
        {
            _updateDto.Name = String.Empty;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Name_Null()
        {
            _updateDto.Name = null;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Name_Length()
        {
            string name = Guid.NewGuid().ToString();
            _updateDto.Name = name;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Uğur")]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_Name_Format_1(string name)
        {
            _updateDto.Name = name;
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Arın")]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_Name_Format_2(string name)
        {
            _updateDto.Name = name;
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Surname_Empty()
        {
            _updateDto.Name = String.Empty;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Surname_Null()
        {
            _updateDto.Name = null;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Surname_Length()
        {
            string name = Guid.NewGuid().ToString();
            _updateDto.Name = name;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Theory]
        [InlineData("Timurcin")]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_Surname(string surname)
        {
            _updateDto.Surname = surname;
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Email_Empty()
        {
            _updateDto.Email = String.Empty;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_Email_Null()
        {
            _updateDto.Email = null;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("ugurtimurcin@.com")]
        public void UpdateValidator_Should_Have_Validation_Error_For_Email_Format(string email)
        {
            _updateDto.Email = email;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("ugurtimurcin@boniglobal.com", 1, 2)]
        public void UpdateValidator_Should_Have_Validation_Error_For_Email_NonUniqueEmail(string email, int userId, int existUserId)
        {
            _updateDto.Email = email;
            _updateDto.Id = userId;
            _panelUserServiceMock.Setup(x => x.GetByEmail(email)).Returns(new Entities.PanelUser() { Id = existUserId});
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Messages.NonUniqueEmail);
        }

        [Theory]
        [InlineData("ugurtimurcin@boniglobal.com")]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_Email(string email)
        {
            _updateDto.Email = email;
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("ugurtimurcin@boniglobal.com", 1, 1)]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_Email_1(string email, int userId, int existUserId)
        {
            _updateDto.Id = userId;
            _updateDto.Email = email;
            _panelUserServiceMock.Setup(x => x.GetByEmail(email)).Returns(new Entities.PanelUser { Id = existUserId });
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void UpdateValidator_Should_Have_Validation_Error_For_UserType()
        {
            _updateDto.UserType = (Types)6;
            _updateValidator.TestValidate(_updateDto).ShouldHaveValidationErrorFor(x => x.UserType);
        }

        [Theory]
        [InlineData(1)]
        public void UpdateValidator_Should_Not_Have_Validation_Error_For_UserType(int type)
        {
            _updateDto.UserType = (Types)type;
            _updateValidator.TestValidate(_updateDto).ShouldNotHaveValidationErrorFor(x => x.UserType);

        }

        [Theory]
        [InlineData("123")]
        public void ResetPasswordValidator_Should_Have_Validation_Error_For_Password(string password)
        {
            _resetPassword.Password = password;
            _resetPasswordValidator.TestValidate(_resetPassword).ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("1234")]
        public void ResetPasswordValidator_Should_Have_Validation_Error_For_Password_PasswordWithoutLetter(string password)
        {
            _resetPassword.Password = password;
            _resetPasswordValidator.TestValidate(_resetPassword).ShouldHaveValidationErrorFor(x => x.Password)
                                               .WithErrorMessage(Messages.PasswordWithoutLetter);
        }

        [Theory]
        [InlineData("ugur")]
        public void ResetPasswordValidator_Should_Have_Validation_Error_For_Password_PasswordWithoutNumber(string password)
        {
            _resetPassword.Password = password;
            _resetPasswordValidator.TestValidate(_resetPassword).ShouldHaveValidationErrorFor(x => x.Password)
                                               .WithErrorMessage(Messages.PasswordWithoutNumber);
        }

        [Theory]
        [InlineData("ugur123")]
        public void ResetPasswordValidator_Should_Not_Have_Validation_Error(string password)
        {
            _resetPassword.Password = password;
            _resetPasswordValidator.TestValidate(_resetPassword).ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
