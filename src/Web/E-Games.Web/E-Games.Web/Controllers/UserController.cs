using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the user profile.
        /// </summary>
        /// <returns>Returns user profile details.</returns>
        /// <response code="200">Returns user profile details</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("User profile requested for user {UserId}", userId);

            var userProfile = await _userService.GetUserProfileAsync(userId!);

            return Ok(userProfile);
        }

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UserProfile
        ///     {
        ///        "username": "user123",
        ///        "phoneNumber": "123-123-123",
        ///        "addressDelivery": "123 Street Name"
        ///     }
        /// </remarks>
        /// <param name="model">UpdateUserModel object</param>
        /// <returns>Returns a status message</returns>
        /// <response code="200">Profile updated successfully</response>
        /// <response code="400">If the model is not valid</response> 
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateProfileAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updateDto = _mapper.Map<UpdateUserModelDto>(model);

            var updatedUserDto = await _userService.UpdateProfileAsync(userId!, updateDto);
            _logger.LogInformation("User update requested for user {UserId}", userId);

            return Ok(updatedUserDto);

        }

        /// <summary>
        /// Updates the user's password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /password
        ///     {
        ///        "currentPassword": "Password1!",
        ///        "newPassword": "Password12!"
        ///     }
        /// </remarks>
        /// <param name="model">UpdatePasswordModel object</param>
        /// <returns>Returns a no content status message </returns>
        /// <response code="204">Password updated successfully</response>
        /// <response code="400">If the model is not valid</response>
        [HttpPatch("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdatePasswordAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updateDto = _mapper.Map<UpdatePasswordModelDto>(model);

            await _userService.UpdatePasswordAsync(userId!, updateDto);
            _logger.LogInformation("Password update requested for user {UserId}", userId);

            return NoContent();
        }
    }
}
