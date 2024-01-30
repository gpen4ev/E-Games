using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Exceptions;
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
        private readonly ICloudinaryService _cloudinaryService;

        public GamesController(IGameService gameService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _gameService = gameService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProducModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: use logger
                return BadRequest(ModelState);
            }

            var productDto = _mapper.Map<CreateProductDto>(model);

            if (model.Logo != null)
            {
                productDto.Logo = await _cloudinaryService.UploadImageAsync(model.Logo);
            }
            if (model.Background != null)
            {
                productDto.Background = await _cloudinaryService.UploadImageAsync(model.Background);
            }

            var createdProduct = await _gameService.CreateProductAsync(productDto);

            if (createdProduct == null)
            {
                return Problem("Product creation failed");
                // TODO: throw errors in service not here
                // TODO: what if enums values do not exist upon filling them in UI?? verify them in service
            }

            var createdProductViewModel = _mapper.Map<CreateProducModel>(createdProduct);

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
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductAsync([FromForm] UpdateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: use logger
                return BadRequest(ModelState);
            }

            if (model.LogoFile != null)
            {
                var logoUrl = await _cloudinaryService.UploadImageAsync(model.LogoFile);
                if (!string.IsNullOrEmpty(logoUrl))
                {
                    model.Logo = logoUrl;
                }
            }

            if (model.BackgroundImageFile != null)
            {
                var backgroundUrl = await _cloudinaryService.UploadImageAsync(model.BackgroundImageFile);
                if (!string.IsNullOrEmpty(backgroundUrl))
                {
                    model.Background = backgroundUrl;
                }
            }

            var productDto = _mapper.Map<UpdateProductDto>(model);

            var updatedProduct = await _gameService.UpdateProductAsync(productDto);

            if (updatedProduct == null)
            {
                return NotFound();
            }

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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _gameService.DeleteProductAsync(id);

            if (!result)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "Product not found");
            }

            return NoContent();
        }
    }
}
