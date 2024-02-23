using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Infrastructure.Filters;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/games")]
    [EnableCors("AllowAll")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService, IMapper mapper, ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _mapper = mapper;
            _logger = logger;
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
        ///     GET /search?term=witcher&amp;limit=10&amp;offset=0
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

        /// <summary>
        /// Retrieves detailed information about a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The detailed information about the product.</returns>
        /// <response code="200">Returns detailed product information</response>
        /// <response code="404">If the product is not found</response>
        [HttpGet("id/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var productDto = await _gameService.GetProductByIdAsync(id);
            var productViewModel = _mapper.Map<FullProductInfoModel>(productDto);

            return Ok(productViewModel);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="model">The product model for creation.</param>
        /// <returns>The created product model.</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the model is not valid</response>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateProductAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var productDto = _mapper.Map<CreateProductDto>(model);
            var createdProduct = await _gameService.CreateProductAsync(productDto);
            var createdProductViewModel = _mapper.Map<CreateProductModel>(createdProduct);

            return Created(string.Empty, createdProductViewModel);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="model">The product model for update.</param>
        /// <returns>The updated product model.</returns>
        /// <response code="200">Returns the updated product</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="404">If the product is not found</response>
        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductAsync([FromForm] UpdateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateProductAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var productDto = _mapper.Map<UpdateProductDto>(model);
            var updatedProduct = await _gameService.UpdateProductAsync(productDto);
            var updatedProductViewModel = _mapper.Map<UpdateProductModel>(updatedProduct);

            return Ok(updatedProductViewModel);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">Product deleted successfully</response>
        /// <response code="404">If the product is not found</response>
        [HttpDelete("id/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _gameService.DeleteProductAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing product rating.
        /// </summary>
        /// <param name="model">The product rating model for update.</param>
        /// <returns>The updated product rating model.</returns>
        /// <response code="200">Returns the updated product rating</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="404">If the product is not found</response>
        [HttpPost("rating")]
        [Authorize]
        public async Task<IActionResult> UpdateRatingAsync([FromBody] EditRatingModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateRatingAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var ratingDto = _mapper.Map<EditRatingDto>(model);
            var updatedRating = await _gameService.UpdateRatingAsync(ratingDto);
            var updatedRatingViewModel = _mapper.Map<EditRatingModel>(updatedRating);

            return Ok(updatedRatingViewModel);
        }


        /// <summary>
        /// Removes a product rating.
        /// </summary>
        /// <returns>No content if the removing is successful.</returns>
        /// <response code="204">Product removed successfully</response>
        /// <response code="404">If the product rating is not updated</response>
        [HttpDelete("rating")]
        [Authorize]
        public async Task<IActionResult> RemoveRatingAsync(string gameName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _gameService.RemoveRatingAsync(gameName, userId!);

            if (!result)
            {
                _logger.LogWarning("RemoveRatingAsync was not able to update rating.");
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Retrieves platforms based on query parameters.
        /// </summary>
        /// <returns>A list of platforms basad on query parameters.</returns>
        /// <response code="200">Returns platforms basad on query parameters</response>
        [HttpGet("list")]
        [ValidateSortFilterParamsAttribute]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters queryParams)
        {
            var result = await _gameService.GetProductsAsync(queryParams);

            return Ok(result);
        }
    }
}
