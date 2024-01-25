using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Data.Data.Models;
using E_Games.Web.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace E_Games.Services.E_Games.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserProfileModelDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound);
            }

            return _mapper.Map<UserProfileModelDto>(user);
        }

        public async Task<UpdateUserModelDto> UpdateProfileAsync(string userId, UpdateUserModelDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "User not found");
            }

            _mapper.Map(model, user);

            var result = await _userManager.UpdateAsync(user!);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description);
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, errorMessages);
            }

            return _mapper.Map<UpdateUserModelDto>(user);
        }

        public async Task UpdatePasswordAsync(string userId, UpdatePasswordModelDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "User not found");
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user!, model.CurrentPassword!);
            if (!checkPassword)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, "Current password is not correct");
            }

            var result = await _userManager.ChangePasswordAsync(user!, model.CurrentPassword!, model.NewPassword!);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description);
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, errorMessages);
            }
        }
    }
}
