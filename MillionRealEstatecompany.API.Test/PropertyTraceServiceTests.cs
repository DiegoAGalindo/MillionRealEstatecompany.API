using AutoMapper;
using FluentAssertions;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Services;
using Moq;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para PropertyTraceService
    /// Siguiendo los principios de TDD y las mejores prácticas de Robert C. Martin
    /// </summary>
    [TestFixture]
    public class PropertyTraceServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IPropertyTraceRepository> _mockPropertyTraceRepository;
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IMapper> _mockMapper;
        private PropertyTraceService _propertyTraceService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPropertyTraceRepository = new Mock<IPropertyTraceRepository>();
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(x => x.PropertyTraces).Returns(_mockPropertyTraceRepository.Object);
            _mockUnitOfWork.Setup(x => x.Properties).Returns(_mockPropertyRepository.Object);

            _propertyTraceService = new PropertyTraceService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region GetTracesByPropertyAsync Tests

        [Test]
        public async Task GetTracesByPropertyAsync_ShouldReturnTraces_WhenTracesExist()
        {
            // Arrange
            var propertyId = 1;
            var traces = new List<PropertyTrace>
            {
                new PropertyTrace { IdPropertyTrace = 1, Name = "Initial Sale", Value = 90000, Tax = 5000, IdProperty = propertyId },
                new PropertyTrace { IdPropertyTrace = 2, Name = "Second Sale", Value = 95000, Tax = 5500, IdProperty = propertyId }
            };
            var traceDtos = new List<PropertyTraceDto>
            {
                new PropertyTraceDto { IdPropertyTrace = 1, Name = "Initial Sale", Value = 90000, Tax = 5000 },
                new PropertyTraceDto { IdPropertyTrace = 2, Name = "Second Sale", Value = 95000, Tax = 5500 }
            };

            _mockPropertyTraceRepository.Setup(x => x.GetTracesByPropertyAsync(propertyId)).ReturnsAsync(traces);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyTraceDto>>(traces)).Returns(traceDtos);

            // Act
            var result = await _propertyTraceService.GetTracesByPropertyAsync(propertyId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(traceDtos);
        }

        [Test]
        public async Task GetTracesByPropertyAsync_ShouldReturnEmptyList_WhenNoTracesExist()
        {
            // Arrange
            var propertyId = 1;
            var emptyTraces = new List<PropertyTrace>();
            var emptyTraceDtos = new List<PropertyTraceDto>();

            _mockPropertyTraceRepository.Setup(x => x.GetTracesByPropertyAsync(propertyId)).ReturnsAsync(emptyTraces);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyTraceDto>>(emptyTraces)).Returns(emptyTraceDtos);

            // Act
            var result = await _propertyTraceService.GetTracesByPropertyAsync(propertyId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetTraceByIdAsync Tests

        [Test]
        public async Task GetTraceByIdAsync_ShouldReturnTrace_WhenTraceExists()
        {
            // Arrange
            var traceId = 1;
            var trace = new PropertyTrace { IdPropertyTrace = traceId, Name = "Test Sale", Value = 100000, Tax = 6000 };
            var traceDto = new PropertyTraceDto { IdPropertyTrace = traceId, Name = "Test Sale", Value = 100000, Tax = 6000 };

            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync(trace);
            _mockMapper.Setup(x => x.Map<PropertyTraceDto>(trace)).Returns(traceDto);

            // Act
            var result = await _propertyTraceService.GetTraceByIdAsync(traceId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(traceDto);
        }

        [Test]
        public async Task GetTraceByIdAsync_ShouldReturnNull_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync((PropertyTrace?)null);

            // Act
            var result = await _propertyTraceService.GetTraceByIdAsync(traceId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region CreatePropertyTraceAsync Tests

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldCreateTrace_WhenValidData()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Christmas Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };
            var newTrace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Christmas Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };
            var createdTraceDto = new PropertyTraceDto
            {
                IdPropertyTrace = 1,
                Name = "Christmas Sale",
                Value = 105000,
                Tax = 6500
            };

            _mockPropertyRepository.Setup(x => x.ExistsAsync(createDto.IdProperty.Value)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<PropertyTrace>(createDto)).Returns(newTrace);
            _mockPropertyTraceRepository.Setup(x => x.AddAsync(It.IsAny<PropertyTrace>()))
                .Callback<PropertyTrace>(trace => trace.IdPropertyTrace = 1)
                .ReturnsAsync(newTrace);
            _mockMapper.Setup(x => x.Map<PropertyTraceDto>(It.IsAny<PropertyTrace>())).Returns(createdTraceDto);

            // Act
            var result = await _propertyTraceService.CreatePropertyTraceAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(createdTraceDto);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldThrowArgumentException_WhenDateSaleIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = null,
                Name = "Invalid Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };

            // Act & Assert
            var act = () => _propertyTraceService.CreatePropertyTraceAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Sale date is required*");
        }

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldThrowArgumentException_WhenValueIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Invalid Sale",
                Value = null,
                Tax = 6500,
                IdProperty = 1
            };

            // Act & Assert
            var act = () => _propertyTraceService.CreatePropertyTraceAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Transaction value is required*");
        }

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldThrowArgumentException_WhenTaxIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Invalid Sale",
                Value = 105000,
                Tax = null,
                IdProperty = 1
            };

            // Act & Assert
            var act = () => _propertyTraceService.CreatePropertyTraceAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Tax is required*");
        }

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldThrowArgumentException_WhenIdPropertyIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Invalid Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = null
            };

            // Act & Assert
            var act = () => _propertyTraceService.CreatePropertyTraceAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Property ID is required*");
        }

        [Test]
        public async Task CreatePropertyTraceAsync_ShouldThrowArgumentException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Invalid Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 999
            };

            _mockPropertyRepository.Setup(x => x.ExistsAsync(createDto.IdProperty.Value)).ReturnsAsync(false);

            // Act & Assert
            var act = () => _propertyTraceService.CreatePropertyTraceAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Property not found*");
        }

        #endregion

        #region UpdatePropertyTraceAsync Tests

        [Test]
        public async Task UpdatePropertyTraceAsync_ShouldUpdateTrace_WhenValidData()
        {
            // Arrange
            var traceId = 1;
            var updateDto = new UpdatePropertyTraceDto
            {
                Name = "Updated Sale",
                Value = 110000,
                Tax = 7000,
                IdProperty = 1
            };
            var existingTrace = new PropertyTrace
            {
                IdPropertyTrace = traceId,
                Name = "Original Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };
            var updatedTraceDto = new PropertyTraceDto
            {
                IdPropertyTrace = traceId,
                Name = "Updated Sale",
                Value = 110000,
                Tax = 7000
            };

            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync(existingTrace);
            _mockPropertyRepository.Setup(x => x.ExistsAsync(updateDto.IdProperty)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map(updateDto, existingTrace));
            _mockMapper.Setup(x => x.Map<PropertyTraceDto>(existingTrace)).Returns(updatedTraceDto);

            // Act
            var result = await _propertyTraceService.UpdatePropertyTraceAsync(traceId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedTraceDto);
            _mockPropertyTraceRepository.Verify(x => x.UpdateAsync(existingTrace), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdatePropertyTraceAsync_ShouldReturnNull_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            var updateDto = new UpdatePropertyTraceDto
            {
                Name = "Updated Sale",
                Value = 110000,
                Tax = 7000,
                IdProperty = 1
            };

            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync((PropertyTrace?)null);

            // Act
            var result = await _propertyTraceService.UpdatePropertyTraceAsync(traceId, updateDto);

            // Assert
            result.Should().BeNull();
            _mockPropertyTraceRepository.Verify(x => x.UpdateAsync(It.IsAny<PropertyTrace>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task UpdatePropertyTraceAsync_ShouldThrowArgumentException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var traceId = 1;
            var updateDto = new UpdatePropertyTraceDto
            {
                Name = "Updated Sale",
                Value = 110000,
                Tax = 7000,
                IdProperty = 999
            };
            var existingTrace = new PropertyTrace
            {
                IdPropertyTrace = traceId,
                Name = "Original Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };

            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync(existingTrace);
            _mockPropertyRepository.Setup(x => x.ExistsAsync(updateDto.IdProperty)).ReturnsAsync(false);

            // Act & Assert
            var act = () => _propertyTraceService.UpdatePropertyTraceAsync(traceId, updateDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Property not found*");
        }

        #endregion

        #region DeletePropertyTraceAsync Tests

        [Test]
        public async Task DeletePropertyTraceAsync_ShouldDeleteTrace_WhenTraceExists()
        {
            // Arrange
            var traceId = 1;
            var existingTrace = new PropertyTrace
            {
                IdPropertyTrace = traceId,
                Name = "Test Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = 1
            };

            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync(existingTrace);

            // Act
            var result = await _propertyTraceService.DeletePropertyTraceAsync(traceId);

            // Assert
            result.Should().BeTrue();
            _mockPropertyTraceRepository.Verify(x => x.DeleteAsync(existingTrace), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeletePropertyTraceAsync_ShouldReturnFalse_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = 999;
            _mockPropertyTraceRepository.Setup(x => x.GetByIdAsync(traceId)).ReturnsAsync((PropertyTrace?)null);

            // Act
            var result = await _propertyTraceService.DeletePropertyTraceAsync(traceId);

            // Assert
            result.Should().BeFalse();
            _mockPropertyTraceRepository.Verify(x => x.DeleteAsync(It.IsAny<PropertyTrace>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        #endregion
    }
}