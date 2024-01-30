using Microsoft.AspNetCore.Http;

namespace E_Games.Services.E_Games.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
