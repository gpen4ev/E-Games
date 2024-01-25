using AutoMapper;
using E_Games.Common;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace E_Games.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task UserProfile_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);
            var userId = Guid.NewGuid().ToString();

            var testProfile = new UserProfileModelDto();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.GetUserProfileAsync(It.IsAny<string>())).ReturnsAsync(testProfile);

            // Act
            var result = await controller.UserProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(testProfile, okResult.Value);
        }

        [Fact]
        public async Task UserProfile_ThrowsApiExceptionBase_WhenUserNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);

            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.GetUserProfileAsync(It.IsAny<string>()))
                           .ThrowsAsync(new ApiExceptionBase(StatusCodes.Status404NotFound, "Not Found"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(() => controller.UserProfile());

            Assert.Equal("Not Found", exception.Message);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);
            var userId = Guid.NewGuid().ToString();

            var updateUserModel = new UpdateUserModel
            {
                UserName = "NewUsername",
                PhoneNumber = "+12345678901",
                AddressDelivery = "456 New Street, New City, Country"
            };

            var expectedUpdateUserDto = new UpdateUserModelDto
            {
                UserName = "NewUsername",
                PhoneNumber = "+12345678901",
                AddressDelivery = "456 New Street, New City, Country"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.UpdateProfileAsync(It.IsAny<string>(), It.IsAny<UpdateUserModelDto>()))
                           .ReturnsAsync(expectedUpdateUserDto);
            mockMapper.Setup(m => m.Map<UpdateUserModelDto>(It.IsAny<UpdateUserModel>()))
                      .Returns(new UpdateUserModelDto());

            // Act
            var result = await controller.UpdateProfileAsync(updateUserModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(expectedUpdateUserDto, okResult.Value as UpdateUserModelDto);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsNotFoundResult_WhenUserNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);

            var userId = Guid.NewGuid().ToString();
            var updateUserModel = new UpdateUserModel
            {
                UserName = "NewUsername",
                PhoneNumber = "+12345678901",
                AddressDelivery = "456 New Street, New City, Country"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.UpdateProfileAsync(It.IsAny<string>(), It.IsAny<UpdateUserModelDto>()))
                           .ThrowsAsync(new ApiExceptionBase(StatusCodes.Status404NotFound, "User not found"));

            // Act
            var result = await controller.UpdateProfileAsync(updateUserModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
            Assert.Equal("User not found", statusCodeResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNoContentResult_WhenPasswordIsUpdated()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);
            var userId = Guid.NewGuid().ToString();

            var updatePasswordModel = new UpdatePasswordModel
            {
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.UpdatePasswordAsync(userId, It.IsAny<UpdatePasswordModelDto>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdatePasswordAsync(updatePasswordModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsBadRequestResult_WhenCurrentPasswordIsIncorrect()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockUserService.Object, mockMapper.Object);

            var userId = Guid.NewGuid().ToString();
            var updatePasswordModel = new UpdatePasswordModel
            {
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            mockUserService.Setup(s => s.UpdatePasswordAsync(userId, It.IsAny<UpdatePasswordModelDto>()))
                           .ThrowsAsync(new ApiExceptionBase(StatusCodes.Status400BadRequest, "Current password is not correct"));

            // Act
            var result = await controller.UpdatePasswordAsync(updatePasswordModel);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal("Current password is not correct", objectResult.Value);
        }
    }
}
