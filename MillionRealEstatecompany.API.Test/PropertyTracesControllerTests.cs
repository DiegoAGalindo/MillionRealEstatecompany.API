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
    /// Pruebas unitarias para PropertyTracesController
    /// Implementando patrones de Clean Architecture y principios SOLID
    /// </summary>
    [TestFixture]
    public class PropertyTracesControllerTests
    {
        private Mock<IPropertyTraceService> _mockPropertyTraceService;
        private Mock<ILogger<PropertyTracesController>> _mockLogger;
        private PropertyTracesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPropertyTraceService = new Mock<IPropertyTraceService>();
            _mockLogger = new Mock<ILogger<PropertyTracesController>>();
            _controller = new PropertyTracesController(_mockPropertyTraceService.Object, _mockLogger.Object);
        }

        #region GetTracesByProperty Tests

        [Test]
        public async Task GetTracesByProperty_ShouldReturnOkWithTraces_WhenTracesExist()
        {
            // Arrange
            var propertyId = 1;
            var traces = new List<PropertyTraceDto>
            {
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = 1, 
                    DateSale = DateTime.Now.Date, 
                    Name = "Trace 1", 
                    Value = 100000,
                    Tax = 5000
                },
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = 2, 
                    DateSale = DateTime.Now.Date.AddDays(-30), 
                    Name = "Trace 2", 
                    Value = 200000,
                    Tax = 10000
                }
            };

            _mockPropertyTraceService.Setup(x => x.GetTracesByPropertyAsync(propertyId)).ReturnsAsync(traces);

            // Act
            var result = await _controller.GetTracesByProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(traces);
        }

        [Test]
        public async Task GetTracesByProperty_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyTraceService.Setup(x => x.GetTracesByPropertyAsync(propertyId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTracesByProperty(propertyId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetTrace Tests

        [Test]
        public async Task GetTrace_ShouldReturnOkWithTrace_WhenTraceExists()
        {
            // Arrange
            var traceId = 1;
            var trace = new PropertyTraceDto 
            { 
                IdPropertyTrace = traceId, 
                DateSale = DateTime.Now.Date, 
                Name = "Test Trace", 
                Value = 150000,
                Tax = 7500
            };

            _mockPropertyTraceService.Setup(x => x.GetTraceByIdAsync(traceId)).ReturnsAsync(trace);

            // Act
            var result = await _controller.GetTrace(traceId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(trace);
        }

        [Test]
        public async Task GetTrace_ShouldReturnNotFound_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            _mockPropertyTraceService.Setup(x => x.GetTraceByIdAsync(traceId)).ReturnsAsync((PropertyTraceDto?)null);

            // Act
            var result = await _controller.GetTrace(traceId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetTrace_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var traceId = 1;
            _mockPropertyTraceService.Setup(x => x.GetTraceByIdAsync(traceId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTrace(traceId);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region CreateTrace Tests

        [Test]
        public async Task CreateTrace_ShouldReturnCreatedAtAction_WhenValidData()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = DateTime.Now.Date,
                Name = "New Trace",
                Value = 250000,
                Tax = 12500,
                IdProperty = 1
            };
            var createdTrace = new PropertyTraceDto
            {
                IdPropertyTrace = 1,
                DateSale = createDto.DateSale.Value,
                Name = createDto.Name,
                Value = createDto.Value.Value,
                Tax = createDto.Tax.Value
            };

            _mockPropertyTraceService.Setup(x => x.CreatePropertyTraceAsync(createDto)).ReturnsAsync(createdTrace);

            // Act
            var result = await _controller.CreateTrace(createDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdTrace);
        }

        [Test]
        public async Task CreateTrace_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = DateTime.Now.Date,
                Name = "New Trace",
                Value = 250000,
                Tax = 12500,
                IdProperty = 999
            };

            _mockPropertyTraceService.Setup(x => x.CreatePropertyTraceAsync(createDto))
                .ThrowsAsync(new ArgumentException("Property not found"));

            // Act
            var result = await _controller.CreateTrace(createDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task CreateTrace_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = DateTime.Now.Date,
                Name = "New Trace",
                Value = 250000,
                Tax = 12500,
                IdProperty = 1
            };

            _mockPropertyTraceService.Setup(x => x.CreatePropertyTraceAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateTrace(createDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region UpdateTrace Tests

        [Test]
        public async Task UpdateTrace_ShouldReturnOkWithUpdatedTrace_WhenValidData()
        {
            // Arrange
            var traceId = 1;
            var updateDto = new UpdatePropertyTraceDto
            {
                DateSale = DateTime.Now.Date.AddDays(-10),
                Name = "Updated Trace",
                Value = 300000,
                Tax = 15000
            };
            var updatedTrace = new PropertyTraceDto
            {
                IdPropertyTrace = traceId,
                DateSale = updateDto.DateSale,
                Name = updateDto.Name,
                Value = updateDto.Value,
                Tax = updateDto.Tax
            };

            _mockPropertyTraceService.Setup(x => x.UpdatePropertyTraceAsync(traceId, updateDto)).ReturnsAsync(updatedTrace);

            // Act
            var result = await _controller.UpdateTrace(traceId, updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(updatedTrace);
        }

        [Test]
        public async Task UpdateTrace_ShouldReturnNotFound_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            var updateDto = new UpdatePropertyTraceDto { Name = "Not Found Trace" };

            _mockPropertyTraceService.Setup(x => x.UpdatePropertyTraceAsync(traceId, updateDto)).ReturnsAsync((PropertyTraceDto?)null);

            // Act
            var result = await _controller.UpdateTrace(traceId, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdateTrace_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var traceId = 1;
            var updateDto = new UpdatePropertyTraceDto { Name = "Updated Trace" };

            _mockPropertyTraceService.Setup(x => x.UpdatePropertyTraceAsync(traceId, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateTrace(traceId, updateDto);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion

        #region DeleteTrace Tests

        [Test]
        public async Task DeleteTrace_ShouldReturnNoContent_WhenTraceDeleted()
        {
            // Arrange
            var traceId = 1;
            _mockPropertyTraceService.Setup(x => x.DeletePropertyTraceAsync(traceId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTrace(traceId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteTrace_ShouldReturnNotFound_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            _mockPropertyTraceService.Setup(x => x.DeletePropertyTraceAsync(traceId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTrace(traceId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task DeleteTrace_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var traceId = 1;
            _mockPropertyTraceService.Setup(x => x.DeletePropertyTraceAsync(traceId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteTrace(traceId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        #endregion
    }
}