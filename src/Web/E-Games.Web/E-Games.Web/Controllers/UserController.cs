using E_Games.Data.Data.Models;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var userProfile = new UserProfileModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AddressDelivery = user.AddressDelivery
            };

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
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.AddressDelivery = model.AddressDelivery;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { user.Id, user.Email, user.PhoneNumber, user.AddressDelivery });
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
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword!);

            if (!checkPassword)
            {
                return BadRequest(new { Message = "Current password is not correct" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
