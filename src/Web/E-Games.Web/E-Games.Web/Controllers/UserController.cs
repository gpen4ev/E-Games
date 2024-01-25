using AutoMapper;
using E_Games.Common;
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

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _userService.GetUserProfileAsync(userId!);

            return Ok(userProfile);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updateDto = _mapper.Map<UpdateUserModelDto>(model);

            try
            {
                var updatedUserDto = await _userService.UpdateProfileAsync(userId!, updateDto);

                return Ok(updatedUserDto);
            }
            catch (ApiExceptionBase ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPatch("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updateDto = _mapper.Map<UpdatePasswordModelDto>(model);

            try
            {
                await _userService.UpdatePasswordAsync(userId!, updateDto);
                return NoContent();
            }
            catch (ApiExceptionBase ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}
