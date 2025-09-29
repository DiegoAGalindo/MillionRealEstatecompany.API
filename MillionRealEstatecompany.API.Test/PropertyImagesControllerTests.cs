using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MillionRealEstatecompany.API.Controllers;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using Moq;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para PropertyImagesController
    /// Implementando patrones de Clean Architecture y principios SOLID
    /// </summary>
    [TestFixture]
    public class PropertyImagesControllerTests
    {
        private Mock<IPropertyImageService> _mockPropertyImageService;
        private Mock<ILogger<PropertyImagesController>> _mockLogger;
        private PropertyImagesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPropertyImageService = new Mock<IPropertyImageService>();
            _mockLogger = new Mock<ILogger<PropertyImagesController>>();
            _controller = new PropertyImagesController(_mockPropertyImageService.Object, _mockLogger.Object);
        }

        #region GetImagesByProperty Tests

        [Test]
        public async Task GetImagesByProperty_ShouldReturnOkWithImages_WhenImagesExist()
        {
            // Arrange
            var propertyId = 1;
            var images = new List<PropertyImageDto>
            {
                new PropertyImageDto { IdPropertyImage = 1, File = "image1.jpg", Enabled = true },
                new PropertyImageDto { IdPropertyImage = 2, File = "image2.jpg", Enabled = true }
            };

            _mockPropertyImageService.Setup(x => x.GetImagesByPropertyAsync(propertyId)).ReturnsAsync(images);

            // Act
            var result = await _controller.GetImagesByProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(images);
        }

        [Test]
        public async Task GetImagesByProperty_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyImageService.Setup(x => x.GetImagesByPropertyAsync(propertyId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetImagesByProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetImage Tests

        [Test]
        public async Task GetImage_ShouldReturnOkWithImage_WhenImageExists()
        {
            // Arrange
            var imageId = 1;
            var image = new PropertyImageDto { IdPropertyImage = imageId, File = "test.jpg", Enabled = true };

            _mockPropertyImageService.Setup(x => x.GetImageByIdAsync(imageId)).ReturnsAsync(image);

            // Act
            var result = await _controller.GetImage(imageId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(image);
        }

        [Test]
        public async Task GetImage_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            _mockPropertyImageService.Setup(x => x.GetImageByIdAsync(imageId)).ReturnsAsync((PropertyImageDto?)null);

            // Act
            var result = await _controller.GetImage(imageId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetImage_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var imageId = 1;
            _mockPropertyImageService.Setup(x => x.GetImageByIdAsync(imageId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetImage(imageId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region CreateImage Tests

        [Test]
        public async Task CreateImage_ShouldReturnCreatedAtAction_WhenValidData()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 1
            };
            var createdImage = new PropertyImageDto
            {
                IdPropertyImage = 1,
                File = "newimage.jpg",
                Enabled = true
            };

            _mockPropertyImageService.Setup(x => x.CreatePropertyImageAsync(createDto)).ReturnsAsync(createdImage);

            // Act
            var result = await _controller.CreateImage(createDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdImage);
        }

        [Test]
        public async Task CreateImage_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 999
            };

            _mockPropertyImageService.Setup(x => x.CreatePropertyImageAsync(createDto))
                .ThrowsAsync(new ArgumentException("Property not found"));

            // Act
            var result = await _controller.CreateImage(createDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task CreateImage_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 1
            };

            _mockPropertyImageService.Setup(x => x.CreatePropertyImageAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateImage(createDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region UpdateImage Tests

        [Test]
        public async Task UpdateImage_ShouldReturnOkWithUpdatedImage_WhenValidData()
        {
            // Arrange
            var imageId = 1;
            var updateDto = new UpdatePropertyImageDto
            {
                File = "updated.jpg",
                Enabled = false
            };
            var updatedImage = new PropertyImageDto
            {
                IdPropertyImage = imageId,
                File = "updated.jpg",
                Enabled = false
            };

            _mockPropertyImageService.Setup(x => x.UpdatePropertyImageAsync(imageId, updateDto)).ReturnsAsync(updatedImage);

            // Act
            var result = await _controller.UpdateImage(imageId, updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedImage);
        }

        [Test]
        public async Task UpdateImage_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            var updateDto = new UpdatePropertyImageDto { File = "notfound.jpg" };

            _mockPropertyImageService.Setup(x => x.UpdatePropertyImageAsync(imageId, updateDto)).ReturnsAsync((PropertyImageDto?)null);

            // Act
            var result = await _controller.UpdateImage(imageId, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdateImage_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var imageId = 1;
            var updateDto = new UpdatePropertyImageDto { File = "updated.jpg" };

            _mockPropertyImageService.Setup(x => x.UpdatePropertyImageAsync(imageId, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateImage(imageId, updateDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region DeleteImage Tests

        [Test]
        public async Task DeleteImage_ShouldReturnNoContent_WhenImageDeleted()
        {
            // Arrange
            var imageId = 1;
            _mockPropertyImageService.Setup(x => x.DeletePropertyImageAsync(imageId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteImage(imageId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteImage_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            _mockPropertyImageService.Setup(x => x.DeletePropertyImageAsync(imageId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteImage(imageId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteImage_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var imageId = 1;
            _mockPropertyImageService.Setup(x => x.DeletePropertyImageAsync(imageId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteImage(imageId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion
    }
}