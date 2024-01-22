﻿using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("signIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody] SignModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email!);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password!, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Sign in successful" });
                }
            }

            return Unauthorized(new { Message = "Invalid email or password" });
        }

        [HttpPost("signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpAsync([FromBody] SignModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                try
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(nameof(EmailConfirmAsync),
                        "Auth", new { userId = user.Id, token = token },
                        protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email!,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
                }
                catch (Exception ex)
                {
                    return Ok(new { Message = "User registered successfully, but we could not send a confirmation email." });
                }

                return StatusCode(StatusCodes.Status201Created);
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        [HttpGet("emailConfirm")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }
    }
}
