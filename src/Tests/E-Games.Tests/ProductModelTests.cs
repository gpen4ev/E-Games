using E_Games.Common.DTOs;
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

        [Fact]
        public void EditRatingModel_Validation_FailsOnMissingGameName()
        {
            var model = new EditRatingModel
            {
                // GameName is not set to simulate a missing value
                NewRating = 3
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            var isValid = Validator.TryValidateObject(model, context, validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("GameName"));
        }

        [Fact]
        public void EditRatingModel_Validation_FailsOnInvalidRating()
        {
            var model = new EditRatingModel
            {
                GameName = "Test Game",
                NewRating = 6
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, validationResults, true);

            Assert.False(isValid, "Validation should fail for an invalid rating.");
            Assert.Contains(validationResults, vr => vr.MemberNames.Any(mn => mn == "NewRating"));
        }

        [Fact]
        public void ProductQueryParameters_DefaultValues_AreCorrect()
        {
            var queryParams = new ProductQueryParameters();

            Assert.Equal(1, queryParams.Page);
            Assert.Equal(10, queryParams.PageSize);
            Assert.Empty(queryParams.Genres);
            Assert.Equal("All", queryParams.AgeRange);
            Assert.Equal("Rating", queryParams.SortBy);
            Assert.Equal("asc", queryParams.SortOrder);
        }
    }
}
