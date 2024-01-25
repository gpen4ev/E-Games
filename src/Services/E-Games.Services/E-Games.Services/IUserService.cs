using E_Games.Common.DTOs;

namespace E_Games.Services.E_Games.Services
{
    public interface IUserService
    {
        Task<UserProfileModelDto> GetUserProfileAsync(string userId);

        Task<UpdateUserModelDto> UpdateProfileAsync(string userId, UpdateUserModelDto model);

        Task UpdatePasswordAsync(string userId, UpdatePasswordModelDto model);
    }
}
