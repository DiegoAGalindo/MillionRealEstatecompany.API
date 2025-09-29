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
    /// Pruebas unitarias para PropertyService
    /// Siguiendo los principios de TDD y las mejores prácticas de Robert C. Martin
    /// </summary>
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private Mock<IMapper> _mockMapper;
        private PropertyService _propertyService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(x => x.Properties).Returns(_mockPropertyRepository.Object);
            _mockUnitOfWork.Setup(x => x.Owners).Returns(_mockOwnerRepository.Object);

            _propertyService = new PropertyService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region GetAllPropertiesAsync Tests

        [Test]
        public async Task GetAllPropertiesAsync_ShouldReturnAllProperties_WhenPropertiesExist()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { IdProperty = 1, Name = "Property 1" },
                new Property { IdProperty = 2, Name = "Property 2" }
            };
            var propertyDtos = new List<PropertyDto>
            {
                new PropertyDto { IdProperty = 1, Name = "Property 1" },
                new PropertyDto { IdProperty = 2, Name = "Property 2" }
            };

            _mockPropertyRepository.Setup(x => x.GetPropertiesWithOwnerAsync())
                .ReturnsAsync(properties);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyDto>>(properties))
                .Returns(propertyDtos);

            // Act
            var result = await _propertyService.GetAllPropertiesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(propertyDtos);
        }

        [Test]
        public async Task GetAllPropertiesAsync_ShouldReturnEmptyList_WhenNoPropertiesExist()
        {
            // Arrange
            var properties = new List<Property>();
            var propertyDtos = new List<PropertyDto>();

            _mockPropertyRepository.Setup(x => x.GetPropertiesWithOwnerAsync())
                .ReturnsAsync(properties);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyDto>>(properties))
                .Returns(propertyDtos);

            // Act
            var result = await _propertyService.GetAllPropertiesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetPropertyByIdAsync Tests

        [Test]
        public async Task GetPropertyByIdAsync_ShouldReturnProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            var property = new Property { IdProperty = propertyId, Name = "Test Property" };
            var propertyDto = new PropertyDto { IdProperty = propertyId, Name = "Test Property" };

            _mockPropertyRepository.Setup(x => x.GetPropertyWithDetailsAsync(propertyId))
                .ReturnsAsync(property);
            _mockMapper.Setup(x => x.Map<PropertyDto>(property))
                .Returns(propertyDto);

            // Act
            var result = await _propertyService.GetPropertyByIdAsync(propertyId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(propertyDto);
        }

        [Test]
        public async Task GetPropertyByIdAsync_ShouldReturnNull_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyRepository.Setup(x => x.GetPropertyWithDetailsAsync(propertyId))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _propertyService.GetPropertyByIdAsync(propertyId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region CreatePropertyAsync Tests

        [Test]
        public async Task CreatePropertyAsync_ShouldCreateProperty_WhenValidDataProvided()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test Street",
                Price = 100000m,
                CodeInternal = "PROP001",
                Year = 2023,
                IdOwner = 1
            };

            var property = new Property
            {
                IdProperty = 1,
                Name = createDto.Name,
                Address = createDto.Address,
                Price = createDto.Price.Value,
                CodeInternal = createDto.CodeInternal,
                Year = createDto.Year.Value,
                IdOwner = createDto.IdOwner.Value
            };

            var propertyDto = new PropertyDto
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

            _mockOwnerRepository.Setup(x => x.ExistsAsync(createDto.IdOwner.Value))
                .ReturnsAsync(true);
            _mockPropertyRepository.Setup(x => x.CodeInternalExistsAsync(createDto.CodeInternal, null))
                .ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<Property>(createDto))
                .Returns(property);
            _mockPropertyRepository.Setup(x => x.AddAsync(It.IsAny<Property>()))
                .ReturnsAsync(property);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mockPropertyRepository.Setup(x => x.GetPropertyWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(property);
            _mockMapper.Setup(x => x.Map<PropertyDto>(property))
                .Returns(propertyDto);

            // Act
            var result = await _propertyService.CreatePropertyAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(propertyDto);
            _mockPropertyRepository.Verify(x => x.AddAsync(It.IsAny<Property>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreatePropertyAsync_ShouldThrowArgumentException_WhenPriceIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test Street",
                Price = null, // Invalid
                CodeInternal = "PROP001",
                Year = 2023,
                IdOwner = 1
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _propertyService.CreatePropertyAsync(createDto));
        }

        [Test]
        public void CreatePropertyAsync_ShouldThrowArgumentException_WhenOwnerDoesNotExist()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test Street",
                Price = 100000m,
                CodeInternal = "PROP001",
                Year = 2023,
                IdOwner = 999 // Non-existent owner
            };

            _mockOwnerRepository.Setup(x => x.ExistsAsync(createDto.IdOwner.Value))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _propertyService.CreatePropertyAsync(createDto));
        }

        [Test]
        public void CreatePropertyAsync_ShouldThrowArgumentException_WhenCodeInternalExists()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test Street",
                Price = 100000m,
                CodeInternal = "EXISTING_CODE",
                Year = 2023,
                IdOwner = 1
            };

            _mockOwnerRepository.Setup(x => x.ExistsAsync(createDto.IdOwner.Value))
                .ReturnsAsync(true);
            _mockPropertyRepository.Setup(x => x.CodeInternalExistsAsync(createDto.CodeInternal, null))
                .ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _propertyService.CreatePropertyAsync(createDto));
        }

        #endregion

        #region UpdatePropertyAsync Tests

        [Test]
        public async Task UpdatePropertyAsync_ShouldUpdateProperty_WhenValidDataProvided()
        {
            // Arrange
            var propertyId = 1;
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 Updated Street",
                Price = 150000m,
                CodeInternal = "PROP002",
                Year = 2024,
                IdOwner = 2
            };

            var existingProperty = new Property
            {
                IdProperty = propertyId,
                Name = "Old Property",
                Address = "Old Address"
            };

            var updatedProperty = new Property
            {
                IdProperty = propertyId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                Price = updateDto.Price,
                CodeInternal = updateDto.CodeInternal,
                Year = updateDto.Year,
                IdOwner = updateDto.IdOwner
            };

            var propertyDto = new PropertyDto
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

            _mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(existingProperty);
            _mockOwnerRepository.Setup(x => x.ExistsAsync(updateDto.IdOwner))
                .ReturnsAsync(true);
            _mockPropertyRepository.Setup(x => x.CodeInternalExistsAsync(updateDto.CodeInternal, propertyId))
                .ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map(updateDto, existingProperty));
            _mockPropertyRepository.Setup(x => x.UpdateAsync(existingProperty))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mockPropertyRepository.Setup(x => x.GetPropertyWithDetailsAsync(propertyId))
                .ReturnsAsync(updatedProperty);
            _mockMapper.Setup(x => x.Map<PropertyDto>(updatedProperty))
                .Returns(propertyDto);

            // Act
            var result = await _propertyService.UpdatePropertyAsync(propertyId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(propertyDto);
            _mockPropertyRepository.Verify(x => x.UpdateAsync(existingProperty), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdatePropertyAsync_ShouldReturnNull_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 Updated Street",
                Price = 150000m,
                CodeInternal = "PROP002",
                Year = 2024,
                IdOwner = 2
            };

            _mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _propertyService.UpdatePropertyAsync(propertyId, updateDto);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region DeletePropertyAsync Tests

        [Test]
        public async Task DeletePropertyAsync_ShouldReturnTrue_WhenPropertyIsDeleted()
        {
            // Arrange
            var propertyId = 1;
            var property = new Property { IdProperty = propertyId, Name = "Test Property" };

            _mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync(property);
            _mockPropertyRepository.Setup(x => x.DeleteAsync(property))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _propertyService.DeletePropertyAsync(propertyId);

            // Assert
            result.Should().BeTrue();
            _mockPropertyRepository.Verify(x => x.DeleteAsync(property), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeletePropertyAsync_ShouldReturnFalse_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _propertyService.DeletePropertyAsync(propertyId);

            // Assert
            result.Should().BeFalse();
            _mockPropertyRepository.Verify(x => x.DeleteAsync(It.IsAny<Property>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region PropertyExistsAsync Tests

        [Test]
        public async Task PropertyExistsAsync_ShouldReturnTrue_WhenPropertyExists()
        {
            // Arrange
            var propertyId = 1;
            _mockPropertyRepository.Setup(x => x.ExistsAsync(propertyId))
                .ReturnsAsync(true);

            // Act
            var result = await _propertyService.PropertyExistsAsync(propertyId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task PropertyExistsAsync_ShouldReturnFalse_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999;
            _mockPropertyRepository.Setup(x => x.ExistsAsync(propertyId))
                .ReturnsAsync(false);

            // Act
            var result = await _propertyService.PropertyExistsAsync(propertyId);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}