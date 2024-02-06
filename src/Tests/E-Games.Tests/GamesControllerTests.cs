using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Data.Data.Enums;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace E_Games.Tests
{
    public class GamesControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GamesController>> _mockLogger;

        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GamesController>>();

            _controller = new GamesController(_mockGameService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetTopPlatformsAsync_ReturnsOkResultWithPlatforms()
        {
            // Arrange
            var platformsDto = new List<PlatformPopularityDto>
            {
                new PlatformPopularityDto { PlatformName = "PC", Count = 9 },
                new PlatformPopularityDto { PlatformName = "Xbox", Count = 5 },
                new PlatformPopularityDto { PlatformName = "PlayStation", Count = 3 }
            };

            _mockGameService.Setup(s => s.GetTopPlatformsAsync()).ReturnsAsync(platformsDto);

            var platformsViewModel = new List<PlatformPopularity>
            {
                new PlatformPopularity { PlatformName = "PC", Count = 9 },
                new PlatformPopularity { PlatformName = "Xbox", Count = 5 },
                new PlatformPopularity { PlatformName = "PlayStation", Count = 3 }
            };

            _mockMapper.Setup(m => m.Map<List<PlatformPopularity>>(platformsDto)).Returns(platformsViewModel);

            // Act
            var result = await _controller.GetTopPlatformsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PlatformPopularity>>(okResult.Value);

            Assert.Equal(platformsViewModel.Count, model.Count());
        }

        [Fact]
        public async Task SearchGamesAsync_ReturnsOkResultWithGames()
        {
            // Arrange
            var gamesDto = new List<SearchGameDto>
            {
                new SearchGameDto { Name = "The Witcher 3: Wild Hunt" },
                new SearchGameDto { Name = "The Legend of Zelda" }
            };

            _mockGameService.Setup(s => s.SearchGamesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(gamesDto);

            var gamesViewModel = new List<SearchGame>
            {
                new SearchGame { Name = "The Witcher 3: Wild Hunt" },
                new SearchGame { Name = "The Legend of Zelda" }
            };

            _mockMapper.Setup(m => m.Map<List<SearchGame>>(gamesDto)).Returns(gamesViewModel);

            // Act
            var result = await _controller.SearchGamesAsync("test", 10, 0);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SearchGame>>(okResult.Value);

            Assert.Equal(gamesViewModel.Count, model.Count());
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsCreatedResult_WhenModelIsValid()
        {
            // Arrange
            var productModel = new CreateProductModel { Name = "Name", Genre = "Sports" };
            var productDto = new CreateProductDto { Name = "Name", Genre = "Sports" };

            _mockMapper.Setup(m => m.Map<CreateProductDto>(It.IsAny<CreateProductModel>())).Returns(productDto);
            _mockGameService.Setup(service => service.CreateProductAsync(It.IsAny<CreateProductDto>())).ReturnsAsync(productDto);
            _mockMapper.Setup(m => m.Map<CreateProductModel>(It.IsAny<CreateProductDto>())).Returns(productModel);

            // Act
            var result = await _controller.CreateProductAsync(productModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);

            Assert.Equal(productModel, createdResult.Value);
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.CreateProductAsync(new CreateProductModel());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new UpdateProductModel();
            _controller.ModelState.AddModelError("TestError", "Test error message");

            // Act
            var result = await _controller.UpdateProductAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsUpdatedProduct_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateModel = new UpdateProductModel
            {
                Id = 1,
                Name = "Updated Game",
                TotalRating = 5,
                Genre = "Adventure",
                Platform = Platforms.PC,
            };

            var updatedDto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated Game",
                TotalRating = 5,
                Genre = "Adventure",
                Platform = Platforms.PC
            };

            var updatedViewModel = new UpdateProductModel
            {
                Id = 1,
                Name = "Updated Game",
                TotalRating = 5,
                Genre = "Adventure",
                Platform = Platforms.PC
            };

            _mockMapper.Setup(m => m.Map<UpdateProductDto>(It.IsAny<UpdateProductModel>())).Returns(updatedDto);
            _mockGameService.Setup(s => s.UpdateProductAsync(It.IsAny<UpdateProductDto>())).ReturnsAsync(updatedDto);
            _mockMapper.Setup(m => m.Map<UpdateProductModel>(It.IsAny<UpdateProductDto>())).Returns(updatedViewModel);

            // Act
            var result = await _controller.UpdateProductAsync(updateModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(updatedViewModel, okResult.Value);
        }


        [Fact]
        public async Task DeleteProduct_ReturnsNoContentResult_WhenProductIsDeleted()
        {
            // Arrange
            int productId = 1;

            _mockGameService.Setup(service => service.DeleteProductAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProductAsync(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateRatingAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var errorMessage = "error message";
            _controller.ModelState.AddModelError("key", errorMessage);

            var model = new EditRatingModel();

            // Act
            var result = await _controller.UpdateRatingAsync(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorObject = badRequestResult.Value as SerializableError;

            Assert.NotNull(errorObject);
            Assert.True(errorObject.ContainsKey("key"));
            var errorMessages = errorObject["key"] as string[];
            Assert.NotNull(errorMessages);
            Assert.Contains(errorMessage, errorMessages);
        }

        [Fact]
        public async Task UpdateRatingAsync_ValidModel_ReturnsUpdatedRating()
        {
            // Arrange
            var model = new EditRatingModel
            {
                GameName = "Grand Theft Auto V",
                NewRating = 5
            };

            var ratingDto = new EditRatingDto
            {
                GameName = "Grand Theft Auto V",
                NewRating = 5
            };

            var updatedRatingDto = new EditRatingDto
            {
                GameName = "Grand Theft Auto V",
                NewRating = 5
            };

            _mockMapper.Setup(m => m.Map<EditRatingDto>(It.IsAny<EditRatingModel>())).Returns(ratingDto);
            _mockGameService.Setup(s => s.UpdateRatingAsync(It.IsAny<EditRatingDto>())).ReturnsAsync(updatedRatingDto);
            _mockMapper.Setup(m => m.Map<EditRatingModel>(It.IsAny<EditRatingDto>())).Returns(model);

            // Act
            var result = await _controller.UpdateRatingAsync(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<EditRatingModel>(okResult.Value);

            _mockGameService.Verify(s => s.UpdateRatingAsync(It.IsAny<EditRatingDto>()), Times.Once);
            _mockMapper.Verify(m => m.Map<EditRatingDto>(It.IsAny<EditRatingModel>()), Times.Once);
            _mockMapper.Verify(m => m.Map<EditRatingModel>(It.IsAny<EditRatingDto>()), Times.Once);
        }

        [Fact]
        public async Task RemoveRatingAsync_RatingExists_ReturnsNoContent()
        {
            // Arrange
            var gameName = "Existing Game";
            var testUserId = Guid.NewGuid().ToString();
            this.SimulateUserAuthentication(testUserId);

            _mockGameService.Setup(s => s.RemoveRatingAsync(gameName, testUserId)).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveRatingAsync(gameName);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveRatingAsync_RatingNotFound_ReturnsNotFound()
        {
            // Arrange
            var gameName = "Non-Existing Game";
            var testUserId = Guid.NewGuid().ToString();
            this.SimulateUserAuthentication(testUserId);

            _mockGameService.Setup(s => s.RemoveRatingAsync(gameName, testUserId)).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveRatingAsync(gameName);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProducts_FilterByGenre_ReturnsFilteredProducts()
        {
            // Arrange
            var queryParams = new ProductQueryParameters { Genres = new List<string> { "Strategy" } };
            var expectedProducts = this.SetupExpectedProducts();

            _mockGameService.Setup(s => s.GetProductsAsync(It.Is<ProductQueryParameters>(p => p.Genres.Contains("Strategy"))))
                            .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts(queryParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResult<FullProductInfoDto>>(okResult.Value);

            Assert.All(returnValue.Items!, item => Assert.Equal("Strategy", item.Genre));
        }

        [Fact]
        public async Task GetProducts_SortByRatingDesc_ReturnsCorrectlySortedProducts()
        {
            // Arrange
            var queryParams = new ProductQueryParameters { SortBy = "Rating", SortOrder = "desc" };
            var expectedProducts = this.SetupExpectedProducts();

            _mockGameService.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryParameters>()))
                            .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts(queryParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnValue = Assert.IsType<PagedResult<FullProductInfoDto>>(okResult.Value);
            var ratings = returnValue.Items!.Select(i => i.Rating).ToList();

            Assert.True(ratings.SequenceEqual(ratings.OrderByDescending(x => x)),
                "Products are not sorted by rating in descending order.");
        }

        private void SimulateUserAuthentication(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private PagedResult<FullProductInfoDto> SetupExpectedProducts()
        {
            return new PagedResult<FullProductInfoDto>
            {
                CurrentPage = 1,
                PageSize = 2,
                TotalItems = 2,
                TotalPages = 1,
                Items = new List<FullProductInfoDto>
                {
                    new FullProductInfoDto { Name = "Game 1", Genre = "Strategy", Rating = Rating.FiveStars },
                    new FullProductInfoDto { Name = "Game 2", Genre = "Strategy", Rating = Rating.FourStars }
                }
            };
        }
    }
}
