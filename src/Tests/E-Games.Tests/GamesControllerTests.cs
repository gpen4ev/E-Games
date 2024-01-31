using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

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
    }
}
