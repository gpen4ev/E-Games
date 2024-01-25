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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("User profile requested for user {UserId}", userId);

            var userProfile = await _userService.GetUserProfileAsync(userId!);

            return Ok(userProfile);
        }

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
