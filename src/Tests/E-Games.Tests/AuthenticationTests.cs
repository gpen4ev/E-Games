using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Newtonsoft.Json.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace E_Games.Tests
{
    public class AuthenticationTests
    {
        [Fact]
        public async Task SignUpAsync_UserRegisters_ReturnsSuccess()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("confirmation-token");

            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callback-url");

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var authController = new AuthController(userManagerMock.Object, null!, emailSenderMock.Object)
            {
                ControllerContext = controllerContext,
                Url = urlHelperMock.Object
            };

            var signUpModel = new SignModel
            {
                Email = "testuser@example.com",
                Password = "TestPassword123!"
            };

            // Act
            var result = await authController.SignUpAsync(signUpModel);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);

            userManagerMock.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
            emailSenderMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SignUpAsync_RegistrationFails_ReturnsBadRequest()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Email already exists." }
            };

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            var emailSenderMock = new Mock<IEmailSender>();

            var authController = new AuthController(
                userManagerMock.Object,
                null!,
                emailSenderMock.Object);

            var signUpModel = new SignModel
            {
                Email = "existinguser@example.com",
                Password = "TestPassword123!"
            };

            // Act
            var result = await authController.SignUpAsync(signUpModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);

            // JObject for parsing the result value
            var jObjectValue = JObject.FromObject(badRequestResult.Value);
            var errorsProperty = jObjectValue["Errors"]?.ToObject<List<string>>();

            Assert.NotNull(errorsProperty);
            Assert.Contains("Email already exists.", errorsProperty);
        }

        [Fact]
        public async Task SignUpAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null!, null!, null!, null!);

            var emailSenderMock = new Mock<IEmailSender>();

            var authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, emailSenderMock.Object);
            authController.ModelState.AddModelError("TestError", "This is a test error");

            var signUpModel = new SignModel();

            // Act
            var result = await authController.SignUpAsync(signUpModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task SignInAsync_UserLogsIn_ReturnsSuccess()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var user = new ApplicationUser
            {
                UserName = "testuser@example.com",
                Email = "testuser@example.com"
            };

            userManagerMock.Setup(x => x.FindByEmailAsync("testuser@example.com"))
                .ReturnsAsync(user);

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null!, null!, null!, null!);

            signInManagerMock.Setup(x => x.PasswordSignInAsync(user, "TestPassword123!", false, false))
                .ReturnsAsync(SignInResult.Success);

            var authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, null!);

            var signInModel = new SignModel
            {
                Email = "testuser@example.com",
                Password = "TestPassword123!"
            };

            // Act
            var result = await authController.SignInAsync(signInModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // JObject for parsing the result value
            var jObjectValue = JObject.FromObject(okResult.Value);
            var message = jObjectValue["Message"]?.ToString();

            Assert.Equal("Sign in successful", message);
        }

        [Fact]
        public async Task SignInAsync_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            userManagerMock.Setup(x => x.FindByEmailAsync("testuser@example.com"))
                .ReturnsAsync((ApplicationUser)null!);

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null!, null!, null!, null!);

            var authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, null!);

            var signInModel = new SignModel
            {
                Email = "testuser@example.com",
                Password = "WrongPassword123!"
            };

            // Act
            var result = await authController.SignInAsync(signInModel);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);

            // JObject for parsing the result value
            var jObjectValue = JObject.FromObject(unauthorizedResult.Value);
            var message = jObjectValue["Message"]?.ToString();

            Assert.Equal("Invalid email or password", message);
        }

        [Fact]
        public async Task SignInAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                null!, null!, null!, null!);

            var authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, null!);
            authController.ModelState.AddModelError("TestError", "This is a test error");

            var signInModel = new SignModel();

            // Act
            var result = await authController.SignInAsync(signInModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}
