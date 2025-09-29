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
    /// Pruebas unitarias para PropertiesController
    /// Implementando patrones de Clean Architecture y principios SOLID
    /// </summary>
    [TestFixture]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _mockPropertyService;
        private Mock<ILogger<PropertiesController>> _mockLogger;
        private PropertiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPropertyService = new Mock<IPropertyService>();
            _mockLogger = new Mock<ILogger<PropertiesController>>();
            _controller = new PropertiesController(_mockPropertyService.Object, _mockLogger.Object);
        }

        #region GetAllProperties Tests

        [Test]
        public async Task GetAllProperties_ShouldReturnOkWithProperties_WhenPropertiesExist()
        {
            // Arrange
            var properties = new List<PropertyDto>
            {
                new PropertyDto { IdProperty = 1, Name = "Property 1", Price = 100000 },
                new PropertyDto { IdProperty = 2, Name = "Property 2", Price = 200000 }
            };

            _mockPropertyService.Setup(x => x.GetAllPropertiesAsync()).ReturnsAsync(properties);

            // Act
            var result = await _controller.GetAllProperties();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(properties);
        }

        [Test]
        public async Task GetAllProperties_ShouldReturnEmptyList_WhenNoPropertiesExist()
        {
            // Arrange
            var properties = new List<PropertyDto>();
            _mockPropertyService.Setup(x => x.GetAllPropertiesAsync()).ReturnsAsync(properties);

            // Act
            var result = await _controller.GetAllProperties();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var resultProperties = okResult!.Value as IEnumerable<PropertyDto>;
            resultProperties.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllProperties_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockPropertyService.Setup(x => x.GetAllPropertiesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllProperties();

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetProperty Tests

        [Test]
        public async Task GetProperty_ShouldReturnOkWithProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            var property = new PropertyDto { IdProperty = propertyId, Name = "Test Property", Price = 100000 };
            _mockPropertyService.Setup(x => x.GetPropertyByIdAsync(propertyId)).ReturnsAsync(property);

            // Act
            var result = await _controller.GetProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(property);
        }

        [Test]
        public async Task GetProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyService.Setup(x => x.GetPropertyByIdAsync(propertyId)).ReturnsAsync((PropertyDto?)null);

            // Act
            var result = await _controller.GetProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetProperty_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyService.Setup(x => x.GetPropertyByIdAsync(propertyId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetPropertyWithDetails Tests

        [Test]
        public async Task GetPropertyWithDetails_ShouldReturnOkWithPropertyDetails_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            var propertyDetails = new PropertyDetailDto 
            { 
                IdProperty = propertyId, 
                Name = "Test Property", 
                Price = 100000,
                Images = new List<PropertyImageDto>(),
                Traces = new List<PropertyTraceDto>()
            };
            _mockPropertyService.Setup(x => x.GetPropertyWithDetailsAsync(propertyId)).ReturnsAsync(propertyDetails);

            // Act
            var result = await _controller.GetPropertyWithDetails(propertyId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(propertyDetails);
        }

        [Test]
        public async Task GetPropertyWithDetails_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyService.Setup(x => x.GetPropertyWithDetailsAsync(propertyId)).ReturnsAsync((PropertyDetailDto?)null);

            // Act
            var result = await _controller.GetPropertyWithDetails(propertyId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetPropertiesByOwner Tests

        [Test]
        public async Task GetPropertiesByOwner_ShouldReturnOkWithProperties_WhenOwnerHasProperties()
        {
            // Arrange
            var ownerId = 1;
            var properties = new List<PropertyDto>
            {
                new PropertyDto { IdProperty = 1, Name = "Property 1", IdOwner = ownerId, Price = 100000 },
                new PropertyDto { IdProperty = 2, Name = "Property 2", IdOwner = ownerId, Price = 200000 }
            };
            _mockPropertyService.Setup(x => x.GetPropertiesByOwnerAsync(ownerId)).ReturnsAsync(properties);

            // Act
            var result = await _controller.GetPropertiesByOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(properties);
        }

        [Test]
        public async Task GetPropertiesByOwner_ShouldReturnEmptyList_WhenOwnerHasNoProperties()
        {
            // Arrange
            var ownerId = 1;
            var properties = new List<PropertyDto>();
            _mockPropertyService.Setup(x => x.GetPropertiesByOwnerAsync(ownerId)).ReturnsAsync(properties);

            // Act
            var result = await _controller.GetPropertiesByOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var resultProperties = okResult!.Value as IEnumerable<PropertyDto>;
            resultProperties.Should().BeEmpty();
        }

        #endregion

        #region CreateProperty Tests

        [Test]
        public async Task CreateProperty_ShouldReturnCreatedResult_WhenPropertyIsCreated()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "New Property",
                Address = "123 Test Street",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                IdOwner = 1
            };

            var createdProperty = new PropertyDto
            {
                IdProperty = 1,
                Name = createDto.Name,
                Address = createDto.Address,
                Price = createDto.Price.Value,
                CodeInternal = createDto.CodeInternal,
                Year = createDto.Year.Value,
                IdOwner = createDto.IdOwner.Value,
                OwnerName = "Test Owner"
            };

            _mockPropertyService.Setup(x => x.CreatePropertyAsync(createDto)).ReturnsAsync(createdProperty);

            // Act
            var result = await _controller.CreateProperty(createDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdProperty);
            createdResult.ActionName.Should().Be(nameof(PropertiesController.GetProperty));
            createdResult.RouteValues!["id"].Should().Be(createdProperty.IdProperty);
        }

        [Test]
        public async Task CreateProperty_ShouldReturnBadRequest_WhenArgumentExceptionThrown()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "New Property",
                Address = "123 Test Street",
                Price = 100000,
                CodeInternal = "EXISTING_CODE",
                Year = 2023,
                IdOwner = 1
            };

            _mockPropertyService.Setup(x => x.CreatePropertyAsync(createDto))
                .ThrowsAsync(new ArgumentException("Internal code already exists"));

            // Act
            var result = await _controller.CreateProperty(createDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            var errorResponse = badRequestResult!.Value;
            errorResponse.Should().NotBeNull();
        }

        #endregion

        #region UpdateProperty Tests

        [Test]
        public async Task UpdateProperty_ShouldReturnOkResult_WhenPropertyIsUpdated()
        {
            // Arrange
            var propertyId = 1;
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 Updated Street",
                Price = 150000,
                CodeInternal = "PROP002",
                Year = 2024,
                IdOwner = 2
            };

            var updatedProperty = new PropertyDto
            {
                IdProperty = propertyId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                Price = updateDto.Price,
                CodeInternal = updateDto.CodeInternal,
                Year = updateDto.Year,
                IdOwner = updateDto.IdOwner,
                OwnerName = "Updated Owner"
            };

            _mockPropertyService.Setup(x => x.UpdatePropertyAsync(propertyId, updateDto))
                .ReturnsAsync(updatedProperty);

            // Act
            var result = await _controller.UpdateProperty(propertyId, updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedProperty);
        }

        [Test]
        public async Task UpdateProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 Updated Street",
                Price = 150000,
                CodeInternal = "PROP002",
                Year = 2024,
                IdOwner = 2
            };

            _mockPropertyService.Setup(x => x.UpdatePropertyAsync(propertyId, updateDto))
                .ReturnsAsync((PropertyDto?)null);

            // Act
            var result = await _controller.UpdateProperty(propertyId, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region DeleteProperty Tests

        [Test]
        public async Task DeleteProperty_ShouldReturnNoContent_WhenPropertyIsDeleted()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyService.Setup(x => x.DeletePropertyAsync(propertyId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProperty(propertyId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyService.Setup(x => x.DeletePropertyAsync(propertyId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProperty(propertyId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetPropertiesByOwner_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var ownerId = 1;
            _mockPropertyService.Setup(x => x.GetPropertiesByOwnerAsync(ownerId))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetPropertiesByOwner(ownerId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task GetPropertyWithDetails_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyService.Setup(x => x.GetPropertyWithDetailsAsync(propertyId))
                .ThrowsAsync(new Exception("Service unavailable"));

            // Act
            var result = await _controller.GetPropertyWithDetails(propertyId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task CreateProperty_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000,
                CodeInternal = "TEST001",
                Year = 2023,
                IdOwner = 1
            };

            _mockPropertyService.Setup(x => x.CreatePropertyAsync(createDto))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.CreateProperty(createDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task UpdateProperty_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var propertyId = 1;
            var updateDto = new UpdatePropertyDto { Name = "Updated Property" };

            _mockPropertyService.Setup(x => x.UpdatePropertyAsync(propertyId, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateProperty(propertyId, updateDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task DeleteProperty_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyService.Setup(x => x.DeletePropertyAsync(propertyId))
                .ThrowsAsync(new Exception("Delete failed"));

            // Act
            var result = await _controller.DeleteProperty(propertyId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region UpdatePropertyPrice Tests

        [Test]
        public async Task UpdatePropertyPrice_ShouldReturnOkWithUpdatedProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            var priceUpdateDto = new UpdatePropertyPriceDto { Price = 500000m };
            var updatedProperty = new PropertyDto { IdProperty = propertyId, Name = "Property 1", Price = 500000m };

            _mockPropertyService.Setup(x => x.UpdatePropertyPriceAsync(propertyId, priceUpdateDto.Price))
                .ReturnsAsync(updatedProperty);

            // Act
            var result = await _controller.UpdatePropertyPrice(propertyId, priceUpdateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedProperty);
        }

        [Test]
        public async Task UpdatePropertyPrice_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 1;
            var priceUpdateDto = new UpdatePropertyPriceDto { Price = 500000m };

            _mockPropertyService.Setup(x => x.UpdatePropertyPriceAsync(propertyId, priceUpdateDto.Price))
                .ReturnsAsync((PropertyDto?)null);

            // Act
            var result = await _controller.UpdatePropertyPrice(propertyId, priceUpdateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdatePropertyPrice_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            var propertyId = 1;
            var priceUpdateDto = new UpdatePropertyPriceDto { Price = -100m };

            _mockPropertyService.Setup(x => x.UpdatePropertyPriceAsync(propertyId, priceUpdateDto.Price))
                .ThrowsAsync(new ArgumentException("El precio debe ser mayor a 0"));

            // Act
            var result = await _controller.UpdatePropertyPrice(propertyId, priceUpdateDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region SearchProperties Tests

        [Test]
        public async Task SearchProperties_ShouldReturnOkWithFilteredProperties_WhenFiltersAreApplied()
        {
            // Arrange
            var filter = new PropertySearchFilter 
            { 
                MinPrice = 100000, 
                MaxPrice = 500000, 
                OwnerId = 1 
            };
            var filteredProperties = new List<PropertyDto>
            {
                new PropertyDto { IdProperty = 1, Name = "Property 1", Price = 300000, IdOwner = 1 }
            };

            _mockPropertyService.Setup(x => x.SearchPropertiesAsync(filter))
                .ReturnsAsync(filteredProperties);

            // Act
            var result = await _controller.SearchProperties(filter);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(filteredProperties);
        }

        [Test]
        public async Task SearchProperties_ShouldReturnEmptyList_WhenNoPropertiesMatchFilters()
        {
            // Arrange
            var filter = new PropertySearchFilter { MinPrice = 1000000 };
            var emptyList = new List<PropertyDto>();

            _mockPropertyService.Setup(x => x.SearchPropertiesAsync(filter))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.SearchProperties(filter);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var properties = okResult!.Value as IEnumerable<PropertyDto>;
            properties.Should().BeEmpty();
        }

        [Test]
        public async Task SearchProperties_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var filter = new PropertySearchFilter();
            _mockPropertyService.Setup(x => x.SearchPropertiesAsync(filter))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.SearchProperties(filter);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion
    }
}