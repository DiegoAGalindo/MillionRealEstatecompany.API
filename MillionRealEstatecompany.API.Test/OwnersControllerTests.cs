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
    /// Pruebas unitarias para OwnersController
    /// Implementando patrones de Clean Architecture y principios SOLID
    /// </summary>
    [TestFixture]
    public class OwnersControllerTests
    {
        private Mock<IOwnerService> _mockOwnerService;
        private Mock<ILogger<OwnersController>> _mockLogger;
        private OwnersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOwnerService = new Mock<IOwnerService>();
            _mockLogger = new Mock<ILogger<OwnersController>>();
            _controller = new OwnersController(_mockOwnerService.Object, _mockLogger.Object);
        }

        #region GetAllOwners Tests

        [Test]
        public async Task GetAllOwners_ShouldReturnOkWithOwners_WhenOwnersExist()
        {
            // Arrange
            var owners = new List<OwnerDto>
            {
                new OwnerDto { IdOwner = 1, Name = "John Doe", DocumentNumber = "12345678" },
                new OwnerDto { IdOwner = 2, Name = "Jane Smith", DocumentNumber = "87654321" }
            };

            _mockOwnerService.Setup(x => x.GetAllOwnersAsync()).ReturnsAsync(owners);

            // Act
            var result = await _controller.GetAllOwners();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(owners);
        }

        [Test]
        public async Task GetAllOwners_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockOwnerService.Setup(x => x.GetAllOwnersAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllOwners();

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetOwner Tests

        [Test]
        public async Task GetOwner_ShouldReturnOkWithOwner_WhenOwnerExists()
        {
            // Arrange
            var ownerId = 1;
            var owner = new OwnerDto { IdOwner = ownerId, Name = "John Doe", DocumentNumber = "12345678" };

            _mockOwnerService.Setup(x => x.GetOwnerByIdAsync(ownerId)).ReturnsAsync(owner);

            // Act
            var result = await _controller.GetOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(owner);
        }

        [Test]
        public async Task GetOwner_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            _mockOwnerService.Setup(x => x.GetOwnerByIdAsync(ownerId)).ReturnsAsync((OwnerDto?)null);

            // Act
            var result = await _controller.GetOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetOwner_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var ownerId = 1;
            _mockOwnerService.Setup(x => x.GetOwnerByIdAsync(ownerId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetOwnerByDocumentNumber Tests

        [Test]
        public async Task GetOwnerByDocumentNumber_ShouldReturnOkWithOwner_WhenOwnerExists()
        {
            // Arrange
            var documentNumber = "12345678";
            var owner = new OwnerDto { IdOwner = 1, Name = "John Doe", DocumentNumber = documentNumber };

            _mockOwnerService.Setup(x => x.GetOwnerByDocumentNumberAsync(documentNumber)).ReturnsAsync(owner);

            // Act
            var result = await _controller.GetOwnerByDocumentNumber(documentNumber);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(owner);
        }

        [Test]
        public async Task GetOwnerByDocumentNumber_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var documentNumber = "99999999";
            _mockOwnerService.Setup(x => x.GetOwnerByDocumentNumberAsync(documentNumber)).ReturnsAsync((OwnerDto?)null);

            // Act
            var result = await _controller.GetOwnerByDocumentNumber(documentNumber);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task GetOwnerByDocumentNumber_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var documentNumber = "invalid";
            _mockOwnerService.Setup(x => x.GetOwnerByDocumentNumberAsync(documentNumber))
                .ThrowsAsync(new ArgumentException("Invalid document number"));

            // Act
            var result = await _controller.GetOwnerByDocumentNumber(documentNumber);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetOwnerByDocumentNumber_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var documentNumber = "12345678";
            _mockOwnerService.Setup(x => x.GetOwnerByDocumentNumberAsync(documentNumber))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetOwnerByDocumentNumber(documentNumber);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region CreateOwner Tests

        [Test]
        public async Task CreateOwner_ShouldReturnCreatedAtAction_WhenValidData()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            var createdOwner = new OwnerDto
            {
                IdOwner = 1,
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };

            _mockOwnerService.Setup(x => x.CreateOwnerAsync(createDto)).ReturnsAsync(createdOwner);

            // Act
            var result = await _controller.CreateOwner(createDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdOwner);
            createdResult.ActionName.Should().Be(nameof(_controller.GetOwner));
        }

        [Test]
        public async Task CreateOwner_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                DocumentNumber = "12345678",
                Email = "invalid-email"
            };

            _mockOwnerService.Setup(x => x.CreateOwnerAsync(createDto))
                .ThrowsAsync(new ArgumentException("Invalid email format"));

            // Act
            var result = await _controller.CreateOwner(createDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task CreateOwner_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };

            _mockOwnerService.Setup(x => x.CreateOwnerAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateOwner(createDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region UpdateOwner Tests

        [Test]
        public async Task UpdateOwner_ShouldReturnOkWithUpdatedOwner_WhenValidData()
        {
            // Arrange
            var ownerId = 1;
            var updateDto = new UpdateOwnerDto
            {
                Name = "John Updated",
                Address = "456 Updated St",
                Email = "johnupdated@example.com"
            };
            var updatedOwner = new OwnerDto
            {
                IdOwner = ownerId,
                Name = "John Updated",
                Address = "456 Updated St",
                DocumentNumber = "12345678",
                Email = "johnupdated@example.com"
            };

            _mockOwnerService.Setup(x => x.UpdateOwnerAsync(ownerId, updateDto)).ReturnsAsync(updatedOwner);

            // Act
            var result = await _controller.UpdateOwner(ownerId, updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedOwner);
        }

        [Test]
        public async Task UpdateOwner_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            var updateDto = new UpdateOwnerDto { Name = "Not Found" };

            _mockOwnerService.Setup(x => x.UpdateOwnerAsync(ownerId, updateDto)).ReturnsAsync((OwnerDto?)null);

            // Act
            var result = await _controller.UpdateOwner(ownerId, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdateOwner_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var ownerId = 1;
            var updateDto = new UpdateOwnerDto { Email = "invalid-email" };

            _mockOwnerService.Setup(x => x.UpdateOwnerAsync(ownerId, updateDto))
                .ThrowsAsync(new ArgumentException("Invalid email format"));

            // Act
            var result = await _controller.UpdateOwner(ownerId, updateDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdateOwner_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var ownerId = 1;
            var updateDto = new UpdateOwnerDto { Name = "John Updated" };

            _mockOwnerService.Setup(x => x.UpdateOwnerAsync(ownerId, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateOwner(ownerId, updateDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region DeleteOwner Tests

        [Test]
        public async Task DeleteOwner_ShouldReturnNoContent_WhenOwnerDeleted()
        {
            // Arrange
            var ownerId = 1;
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync(ownerId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteOwner(ownerId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync(ownerId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteOwner(ownerId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturnBadRequest_WhenInvalidOperationException()
        {
            // Arrange
            var ownerId = 1;
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync(ownerId))
                .ThrowsAsync(new InvalidOperationException("Owner has associated properties"));

            // Act
            var result = await _controller.DeleteOwner(ownerId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var ownerId = 1;
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync(ownerId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteOwner(ownerId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion
    }
}