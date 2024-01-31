using E_Games.Data.Data.Enums;
using E_Games.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace E_Games.Tests
{
    public class ProductModelTests
    {
        [Fact]
        public void CreateProductModel_ValidationFails_WhenNameNotProvided()
        {
            // Arrange
            var model = new CreateProductModel
            {
                LogoFile = null,
                BackgroundImageFile = null,
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void UpdateProductModel_CanSetAndGetProperties()
        {
            // Arrange
            var model = new UpdateProductModel();

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("testfile.png");

            // Act
            model.Name = "Test Product";
            model.LogoFile = mockFile.Object;
            model.BackgroundImageFile = mockFile.Object;
            model.Platform = Platforms.PC;
            model.TotalRating = 5;
            model.Genre = "Action";

            // Assert
            Assert.Equal("Test Product", model.Name);
            Assert.Equal("testfile.png", model.LogoFile.FileName);
            Assert.Equal("testfile.png", model.BackgroundImageFile.FileName);
            Assert.Equal(Platforms.PC, model.Platform);
            Assert.Equal(5, model.TotalRating);
            Assert.Equal("Action", model.Genre);
        }
    }
}
