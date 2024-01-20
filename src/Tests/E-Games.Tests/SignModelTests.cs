using E_Games.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace E_Games.Tests
{
    public class SignModelTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData("notanemail", false)]
        [InlineData("email@example.com", true)]
        public void EmailValidationTest(string email, bool isValid)
        {
            // Arrange
            var model = new SignModel
            {
                Email = email,
                Password = "ValidPassword1!"
            };

            // Act
            var validationResult = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            var validateObject = Validator.TryValidateObject(model, validationContext, validationResult, true);

            // Assert
            Assert.Equal(isValid, validateObject);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("short", false)]
        [InlineData("NoSpecialChar1", false)]
        [InlineData("ValidPassword1!", true)]
        public void PasswordValidationTest(string password, bool isValid)
        {
            // Arrange
            var model = new SignModel
            {
                Email = "email@example.com",
                Password = password
            };

            // Act
            var validationResult = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            var validateObject = Validator.TryValidateObject(model, validationContext, validationResult, true);

            // Assert
            Assert.Equal(isValid, validateObject);
        }
    }
}
