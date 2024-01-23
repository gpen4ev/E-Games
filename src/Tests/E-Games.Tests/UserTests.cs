using E_Games.Data.Data.Models;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace E_Games.Tests
{
    public class UserTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly UserController _controller;
        private readonly ApplicationUser _testUser;

        public UserTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

            _controller = new UserController(_userManagerMock.Object);

            var guid = Guid.NewGuid();

            _testUser = new ApplicationUser
            {
                Id = guid,
                UserName = "testUser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                AddressDelivery = "123 Test Address"
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString())
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task UserProfileAsync_UserExists_ReturnsSuccess()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString()))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _controller.UserProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userProfile = Assert.IsType<UserProfileModel>(okResult.Value);

            Assert.Equal(_testUser.Email, userProfile.Email);
            Assert.Equal(_testUser.PhoneNumber, userProfile.PhoneNumber);
            Assert.Equal(_testUser.AddressDelivery, userProfile.AddressDelivery);
        }

        [Fact]
        public async Task UserProfileAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString()))
                .ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _controller.UserProfile();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProfileAsync_UserExists_UpdatesProfile()
        {
            // Arrange
            var updateModel = new UpdateUserModel
            {
                UserName = "Updated User",
                PhoneNumber = "123-234-456",
                AddressDelivery = "New Address"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString()))
                .ReturnsAsync(_testUser);

            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.UpdateProfileAsync(updateModel);
            var okResult = Assert.IsType<OkObjectResult>(result);

            _userManagerMock.Verify(um => um.UpdateAsync(It.Is<ApplicationUser>(u =>
                u.UserName == updateModel.UserName &&
                u.PhoneNumber == updateModel.PhoneNumber &&
                u.AddressDelivery == updateModel.AddressDelivery)), Times.Once);
        }

        [Fact]
        public async Task UpdateProfileAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateModel = new UpdateUserModel
            {
                UserName = "Updated User",
                PhoneNumber = "123-234-456",
                AddressDelivery = "New Address"
            };
            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString())).ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _controller.UpdateProfileAsync(updateModel);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePasswordAsync_ValidUser_UpdatesPassword()
        {
            // Arrange
            var passwordUpdateModel = new UpdatePasswordModel
            {
                CurrentPassword = "oldPassword123",
                NewPassword = "newPassword123"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString()))
                .ReturnsAsync(_testUser);

            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), "oldPassword123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), "oldPassword123", "newPassword123"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.UpdatePasswordAsync(passwordUpdateModel);

            // Assert
            Assert.IsType<NoContentResult>(result);

            _userManagerMock.Verify(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(),
                "oldPassword123", "newPassword123"), Times.Once);
        }

        [Fact]
        public async Task UpdatePasswordAsync_UserNotFoundOrInvalid_ReturnsBadRequest()
        {
            // Arrange
            var passwordUpdateModel = new UpdatePasswordModel
            {
                CurrentPassword = "oldPassword123",
                NewPassword = "newPassword123"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(_testUser.Id.ToString()))
                .ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _controller.UpdatePasswordAsync(passwordUpdateModel);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
