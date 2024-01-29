using E_Games.Common.DTOs;

namespace E_Games.Services.E_Games.Services
{
    public interface IGameService
    {
        Task<List<PlatformPopularityDto>> GetTopPlatformsAsync();

        Task<List<SearchGameDto>> SearchGamesAsync(string term, int limit, int offset);

        Task<FullProductInfoDto> GetProductByIdAsync(int id);

        Task<CreateProductDto> CreateProductAsync(CreateProductDto model);
    }
}
