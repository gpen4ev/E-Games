using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_Games.Tests
{
    public class GamesControllerTests
    {
        [Fact]
        public async Task GetTopPlatformsAsync_ReturnsOkResultWithPlatforms()
        {
            // Arrange
            var mockService = new Mock<IGameService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new GamesController(mockService.Object, mockMapper.Object);

            var platformsDto = new List<PlatformPopularityDto>
            {
                new PlatformPopularityDto { PlatformName = "PC", Count = 9 },
                new PlatformPopularityDto { PlatformName = "Xbox", Count = 5 },
                new PlatformPopularityDto { PlatformName = "PlayStation", Count = 3 }
            };

            mockService.Setup(s => s.GetTopPlatformsAsync()).ReturnsAsync(platformsDto);

            var platformsViewModel = new List<PlatformPopularity>
            {
                new PlatformPopularity { PlatformName = "PC", Count = 9 },
                new PlatformPopularity { PlatformName = "Xbox", Count = 5 },
                new PlatformPopularity { PlatformName = "PlayStation", Count = 3 }
            };

            mockMapper.Setup(m => m.Map<List<PlatformPopularity>>(platformsDto)).Returns(platformsViewModel);

            // Act
            var result = await controller.GetTopPlatformsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PlatformPopularity>>(okResult.Value);

            Assert.Equal(platformsViewModel.Count, model.Count());
        }

        [Fact]
        public async Task SearchGamesAsync_ReturnsOkResultWithGames()
        {
            // Arrange
            var mockService = new Mock<IGameService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new GamesController(mockService.Object, mockMapper.Object);

            var gamesDto = new List<SearchGameDto>
            {
                new SearchGameDto { Name = "The Witcher 3: Wild Hunt" },
                new SearchGameDto { Name = "The Legend of Zelda" }
            };

            mockService.Setup(s => s.SearchGamesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(gamesDto);

            var gamesViewModel = new List<SearchGame>
            {
                new SearchGame { Name = "The Witcher 3: Wild Hunt" },
                new SearchGame { Name = "The Legend of Zelda" }
            };

            mockMapper.Setup(m => m.Map<List<SearchGame>>(gamesDto)).Returns(gamesViewModel);

            // Act
            var result = await controller.SearchGamesAsync("test", 10, 0);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SearchGame>>(okResult.Value);

            Assert.Equal(gamesViewModel.Count, model.Count());
        }
    }
}
