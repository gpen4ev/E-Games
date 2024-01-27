using E_Games.Data.Data;
using E_Games.Data.Data.Enums;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Tests
{
    public class GameServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _gameService = new GameService(_context);
        }

        [Fact]
        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetTopPlatformsAsync_ReturnsTop3Platforms()
        {
            // Arrange
            var testData = new List<Product>
            {
                new Product { Name = "The Witcher 3: Wild Hunt", Platform = Platforms.PC },
                new Product { Name = "Red Dead Redemption 2", Platform = Platforms.Nintendo },
                new Product { Name = "Grand Theft Auto V", Platform = Platforms.Xbox },
            };

            foreach (var product in testData)
            {
                _context.Products.Add(product);
            }

            _context.SaveChanges();

            // Act
            var result = await _gameService.GetTopPlatformsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetTopPlatformsAsync_CorrectPlatformsAndCounts()
        {
            var testData = new List<Product>
            {
                new Product { Name = "The Witcher 3: Wild Hunt", Platform = Platforms.PC },
                new Product { Name = "Dark Souls III", Platform = Platforms.PC },
                new Product { Name = "Bloodborne", Platform = Platforms.PC },
                new Product { Name = "The Last of Us Part II", Platform = Platforms.PC },
                new Product { Name = "God of War", Platform = Platforms.PC },
                new Product { Name = "Red Dead Redemption 2", Platform = Platforms.Nintendo },
                new Product { Name = "The Elder Scrolls V: Skyrim", Platform = Platforms.Nintendo },
                new Product { Name = "Ghost of Tsushima", Platform = Platforms.Nintendo },
                new Product { Name = "Star Wars Jedi: Fallen Order", Platform = Platforms.Nintendo },
                new Product { Name = "Grand Theft Auto V", Platform = Platforms.Xbox },
                new Product { Name = "The Legend of Zelda", Platform = Platforms.Xbox },
                new Product { Name = "Sekiro: Shadows Die Twice", Platform = Platforms.Xbox },
            };

            foreach (var product in testData)
            {
                _context.Products.Add(product);
            }

            _context.SaveChanges();

            // Act
            var result = await _gameService.GetTopPlatformsAsync();

            // Assert
            Assert.Equal("PC", result[0].PlatformName);
            Assert.Equal(5, result[0].Count);

            Assert.Equal("Nintendo", result[1].PlatformName);
            Assert.Equal(4, result[1].Count);

            Assert.Equal("Xbox", result[2].PlatformName);
            Assert.Equal(3, result[2].Count);
        }

        [Fact]
        public async Task GetTopPlatformsAsync_CorrectOrdering()
        {
            // Arrange
            var testData = new List<Product>
            {
                new Product { Name = "The Witcher 3: Wild Hunt", Platform = Platforms.PC },
                new Product { Name = "Dark Souls III", Platform = Platforms.PC },
                new Product { Name = "Bloodborne", Platform = Platforms.PC },
                new Product { Name = "The Last of Us Part II", Platform = Platforms.PC },
                new Product { Name = "God of War", Platform = Platforms.PC },
                new Product { Name = "Grand Theft Auto V", Platform = Platforms.Xbox },
                new Product { Name = "The Legend of Zelda", Platform = Platforms.Xbox },
                new Product { Name = "Sekiro: Shadows Die Twice", Platform = Platforms.Xbox },
                new Product { Name = "Minecraft", Platform = Platforms.PlayStation },
                new Product { Name = "Cyberpunk 2077", Platform = Platforms.PlayStation },
            };

            foreach (var product in testData)
            {
                _context.Products.Add(product);
            }

            _context.SaveChanges();

            // Act
            var result = await _gameService.GetTopPlatformsAsync();

            // Assert
            Assert.True(result[0].Count >= result[1].Count);
            Assert.True(result[1].Count >= result[2].Count);
        }

        [Fact]
        public async Task SearchGamesAsync_ValidParameters_ReturnsMatchingGames()
        {
            // Arrange
            var testData = new List<Product>
            {
                new Product { Name = "Minecraft" },
                new Product { Name = "Minecraft Story Mode" },
            };

            foreach (var product in testData)
            {
                _context.Products.Add(product);
            }

            _context.SaveChanges();

            // Act
            var result = await _gameService.SearchGamesAsync("Minecraft", 10, 0);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, game => Assert.Contains("Minecraft", game.Name));
        }

        [Fact]
        public async Task SearchGamesAsync_EmptyResultHandling()
        {
            // Act
            var result = await _gameService.SearchGamesAsync("Missing Game", 10, 0);

            // Assert
            Assert.Empty(result);
        }
    }
}
