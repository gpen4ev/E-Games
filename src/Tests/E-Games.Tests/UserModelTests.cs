using E_Games.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace E_Games.Tests
{
    public class UserModelTests
    {
        private ValidationContext? _validationContext;
        private List<ValidationResult> _validationResult;

        public UserModelTests()
        {
            _validationResult = new List<ValidationResult>();
        }

        [Theory]
        [InlineData("+12345678901", "123 Street", "TestUser")]
        [InlineData("1234567890", "123 Street", "TestUser")]
        [InlineData("invalidPhoneNumber", "123 Street", "TestUser", false)]
        [InlineData("+12345678901", "", "TestUser", false)]
        [InlineData("+12345678901", "123 Street", "", false)]
        public void UpdateUserModel_Validation(string phoneNumber, string address, string userName, bool isValid = true)
        {
            // Arrange
            var model = new UpdateUserModel
            {
                UserName = userName,
                PhoneNumber = phoneNumber,
                AddressDelivery = address
            };

            _validationContext = new ValidationContext(model);
            _validationResult = new List<ValidationResult>();

            // Act
            var validateObject = Validator.TryValidateObject(model, _validationContext, _validationResult, true);

            // Assert
            Assert.Equal(isValid, validateObject);
        }

        [Theory]
        [InlineData("Passw0rd!", true)]
        [InlineData("passw0rd", false)]
        [InlineData("PASSWORD!", false)]
        [InlineData("Short1!", false)]
        public void UpdatePasswordModel_Validation(string newPassword, bool isValid)
        {
            // Arrange
            var model = new UpdatePasswordModel
            {
                NewPassword = newPassword
            };

            _validationContext = new ValidationContext(model);

            // Act
            var validateObject = Validator.TryValidateObject(model, _validationContext, _validationResult, true);

            // Assert
            Assert.Equal(isValid, validateObject);
        }

        [Fact]
        public void UserProfileModel_Properties_Validation()
        {
            // Arrange
            var model = new UserProfileModel
            {
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                AddressDelivery = "123 Test Address"
            };

            // Act & Assert
            Assert.Equal("test@example.com", model.Email);
            Assert.Equal("1234567890", model.PhoneNumber);
            Assert.Equal("123 Test Address", model.AddressDelivery);
        }
    }
}
