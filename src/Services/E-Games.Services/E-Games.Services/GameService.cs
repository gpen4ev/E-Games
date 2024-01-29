using E_Games.Common.DTOs;
using E_Games.Data.Data;
using E_Games.Web.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Services.E_Games.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context) => _context = context;

        public async Task<List<PlatformPopularityDto>> GetTopPlatformsAsync() => await _context.Products
                .GroupBy(p => p.Platform)
                .Select(group => new PlatformPopularityDto
                {
                    PlatformName = group.Key.ToString(),
                    Count = group.Count()
                })
                .OrderByDescending(dto => dto.Count)
                .Take(3)
                .ToListAsync();

        public async Task<List<SearchGameDto>> SearchGamesAsync(string term, int limit, int offset)
        {
            if (limit <= 0 || offset < 0)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest,
                    "Invalid parameters: Limit must be greater than zero and Offset cannot be negative.");
            }

            var queryResult = await _context.Products
                .Where(p => p.Name!.Contains(term))
                .Skip(offset)
                .Take(limit)
                .Select(p => new SearchGameDto
                {
                    Name = p.Name,
                })
                .ToListAsync();

            return queryResult;
        }
    }
}
