using AutoMapper;
using E_Games.Services.E_Games.Services;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GamesController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the top platforms based on the number of games.
        /// </summary>
        /// <returns>A list of top platforms with their respective game counts.</returns>
        /// <response code="200">Returns the top platforms with game counts</response>
        [HttpGet("topPlatforms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopPlatformsAsync()
        {
            var topPlatformsDto = await _gameService.GetTopPlatformsAsync();
            var topPlatformsViewModel = _mapper.Map<List<PlatformPopularity>>(topPlatformsDto);

            return Ok(topPlatformsViewModel);
        }

        /// <summary>
        /// Searches for games based on the provided term, with pagination support.
        /// </summary>
        /// <param name="term">The search term to filter games.</param>
        /// <param name="limit">The maximum number of results to return.</param>
        /// <param name="offset">The number of results to skip (for pagination).</param>
        /// <returns>A list of games matching the search criteria.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /search?term=witcher&limit=10&offset=0
        ///
        /// </remarks>
        /// <response code="200">Returns the matching games</response>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchGamesAsync(string term, int limit, int offset)
        {
            var searchGamesDto = await _gameService.SearchGamesAsync(term, limit, offset);
            var searchGamesViewModel = _mapper.Map<List<SearchGame>>(searchGamesDto);

            return Ok(searchGamesViewModel);
        }
    }
}
