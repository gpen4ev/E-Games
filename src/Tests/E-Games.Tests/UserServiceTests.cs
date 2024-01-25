using AutoMapper;
using E_Games.Common;
using E_Games.Common.DTOs;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace E_Games.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserProfileAsync_ReturnsUserProfile_WhenUserExists()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var mockMapper = new Mock<IMapper>();
            var userService = new UserService(mockUserManager.Object, mockMapper.Object);

            var userId = Guid.NewGuid();
            var testUser = new ApplicationUser { Id = userId, UserName = "TestUser" };

            mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(testUser);
            mockMapper.Setup(x => x.Map<UserProfileModelDto>(It.IsAny<ApplicationUser>())).Returns(new UserProfileModelDto());

            // Act
            var result = await userService.GetUserProfileAsync(userId.ToString());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserProfileAsync_ThrowsError_WhenUserNotFound()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var mockMapper = new Mock<IMapper>();
            var userService = new UserService(mockUserManager.Object, mockMapper.Object);

            var userId = "nonExistingUserId";

            mockUserManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(() => userService.GetUserProfileAsync(userId));
            Assert.Equal("Not Found", exception.Message);
        }

        [Fact]
        public async Task UpdateProfileAsync_UpdatesProfile_WhenUserExists()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var mockMapper = new Mock<IMapper>();
            var userService = new UserService(mockUserManager.Object, mockMapper.Object);

            var userId = Guid.NewGuid();
            var testUser = new ApplicationUser { Id = userId };

            var updateUserModelDto = new UpdateUserModelDto
            {
                UserName = "NewUsername",
                PhoneNumber = "+1234567890",
                AddressDelivery = "123 New Address, City, Country"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(testUser);
            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mockMapper.Setup(x => x.Map(It.IsAny<UpdateUserModelDto>(), It.IsAny<ApplicationUser>()));

            // Act
            await userService.UpdateProfileAsync(userId.ToString(), updateUserModelDto);

            // Assert
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProfileAsync_ThrowsError_WhenUserNotFound()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var mockMapper = new Mock<IMapper>();
            var userService = new UserService(mockUserManager.Object, mockMapper.Object);

            var userId = Guid.NewGuid().ToString();

            var updateUserModelDto = new UpdateUserModelDto
            {
                UserName = "NewUsername",
                PhoneNumber = "+1234567890",
                AddressDelivery = "123 New Address, City, Country"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(() => userService.UpdateProfileAsync(userId, updateUserModelDto));
            Assert.Equal("Not Found", exception.Message);
        }

        [Fact]
        public async Task UpdatePasswordAsync_UpdatesPassword_WhenCurrentPasswordIsCorrect()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var mockMapper = new Mock<IMapper>();
            var userService = new UserService(mockUserManager.Object, mockMapper.Object);

            var userId = Guid.NewGuid();

            var model = new UpdatePasswordModelDto
            {
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            var testUser = new ApplicationUser { Id = userId };

            mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(testUser);
            mockUserManager.Setup(x => x.CheckPasswordAsync(testUser, model.CurrentPassword)).ReturnsAsync(true);
            mockUserManager.Setup(x => x.ChangePasswordAsync(testUser, model.CurrentPassword, model.NewPassword))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            await userService.UpdatePasswordAsync(userId.ToString(), model);

            // Assert
            mockUserManager.Verify(x => x.ChangePasswordAsync(testUser, model.CurrentPassword, model.NewPassword), Times.Once);
        }

        [Fact]
        public async Task UpdatePasswordAsync_ThrowsError_WhenCurrentPasswordIsIncorrect()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var userService = new UserService(mockUserManager.Object, null!);
            var userId = Guid.NewGuid();

            var model = new UpdatePasswordModelDto
            {
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            var testUser = new ApplicationUser { Id = userId };

            mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(testUser);
            mockUserManager.Setup(x => x.CheckPasswordAsync(testUser, model.CurrentPassword)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(() => userService.UpdatePasswordAsync(userId.ToString(), model));
            Assert.Equal("Bad Request", exception.Message);
        }
    }
}
