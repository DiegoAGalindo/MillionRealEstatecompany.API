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
    /// Pruebas unitarias para PropertyImageService
    /// Siguiendo los principios de TDD y las mejores prácticas de Robert C. Martin
    /// </summary>
    [TestFixture]
    public class PropertyImageServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IPropertyImageRepository> _mockPropertyImageRepository;
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IMapper> _mockMapper;
        private PropertyImageService _propertyImageService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPropertyImageRepository = new Mock<IPropertyImageRepository>();
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(x => x.PropertyImages).Returns(_mockPropertyImageRepository.Object);
            _mockUnitOfWork.Setup(x => x.Properties).Returns(_mockPropertyRepository.Object);

            _propertyImageService = new PropertyImageService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region GetImagesByPropertyAsync Tests

        [Test]
        public async Task GetImagesByPropertyAsync_ShouldReturnImages_WhenImagesExist()
        {
            // Arrange
            var propertyId = 1;
            var images = new List<PropertyImage>
            {
                new PropertyImage { IdPropertyImage = 1, File = "image1.jpg", Enabled = true, IdProperty = propertyId },
                new PropertyImage { IdPropertyImage = 2, File = "image2.jpg", Enabled = true, IdProperty = propertyId }
            };
            var imageDtos = new List<PropertyImageDto>
            {
                new PropertyImageDto { IdPropertyImage = 1, File = "image1.jpg", Enabled = true },
                new PropertyImageDto { IdPropertyImage = 2, File = "image2.jpg", Enabled = true }
            };

            _mockPropertyImageRepository.Setup(x => x.GetImagesByPropertyAsync(propertyId)).ReturnsAsync(images);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyImageDto>>(images)).Returns(imageDtos);

            // Act
            var result = await _propertyImageService.GetImagesByPropertyAsync(propertyId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(imageDtos);
        }

        [Test]
        public async Task GetImagesByPropertyAsync_ShouldReturnEmptyList_WhenNoImagesExist()
        {
            // Arrange
            var propertyId = 1;
            var emptyImages = new List<PropertyImage>();
            var emptyImageDtos = new List<PropertyImageDto>();

            _mockPropertyImageRepository.Setup(x => x.GetImagesByPropertyAsync(propertyId)).ReturnsAsync(emptyImages);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyImageDto>>(emptyImages)).Returns(emptyImageDtos);

            // Act
            var result = await _propertyImageService.GetImagesByPropertyAsync(propertyId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetImageByIdAsync Tests

        [Test]
        public async Task GetImageByIdAsync_ShouldReturnImage_WhenImageExists()
        {
            // Arrange
            var imageId = 1;
            var image = new PropertyImage { IdPropertyImage = imageId, File = "test.jpg", Enabled = true };
            var imageDto = new PropertyImageDto { IdPropertyImage = imageId, File = "test.jpg", Enabled = true };

            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync(image);
            _mockMapper.Setup(x => x.Map<PropertyImageDto>(image)).Returns(imageDto);

            // Act
            var result = await _propertyImageService.GetImageByIdAsync(imageId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(imageDto);
        }

        [Test]
        public async Task GetImageByIdAsync_ShouldReturnNull_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync((PropertyImage?)null);

            // Act
            var result = await _propertyImageService.GetImageByIdAsync(imageId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region CreatePropertyImageAsync Tests

        [Test]
        public async Task CreatePropertyImageAsync_ShouldCreateImage_WhenValidData()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 1
            };
            var newImage = new PropertyImage
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 1
            };
            var createdImageDto = new PropertyImageDto
            {
                IdPropertyImage = 1,
                File = "newimage.jpg",
                Enabled = true
            };

            _mockPropertyRepository.Setup(x => x.ExistsAsync(createDto.IdProperty.Value)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<PropertyImage>(createDto)).Returns(newImage);
            _mockPropertyImageRepository.Setup(x => x.AddAsync(It.IsAny<PropertyImage>()))
                .Callback<PropertyImage>(img => img.IdPropertyImage = 1)
                .ReturnsAsync(newImage);
            _mockMapper.Setup(x => x.Map<PropertyImageDto>(It.IsAny<PropertyImage>())).Returns(createdImageDto);

            // Act
            var result = await _propertyImageService.CreatePropertyImageAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(createdImageDto);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreatePropertyImageAsync_ShouldThrowArgumentException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = 999
            };

            _mockPropertyRepository.Setup(x => x.ExistsAsync(createDto.IdProperty.Value)).ReturnsAsync(false);

            // Act & Assert
            var act = () => _propertyImageService.CreatePropertyImageAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Property not found*");
        }

        [Test]
        public async Task CreatePropertyImageAsync_ShouldThrowArgumentException_WhenIdPropertyIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = true,
                IdProperty = null
            };

            // Act & Assert
            var act = () => _propertyImageService.CreatePropertyImageAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Property ID is required*");
        }

        [Test]
        public async Task CreatePropertyImageAsync_ShouldThrowArgumentException_WhenEnabledIsNull()
        {
            // Arrange
            var createDto = new CreatePropertyImageDto
            {
                File = "newimage.jpg",
                Enabled = null,
                IdProperty = 1
            };

            // Act & Assert
            var act = () => _propertyImageService.CreatePropertyImageAsync(createDto);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Enabled status is required*");
        }

        #endregion

        #region UpdatePropertyImageAsync Tests

        [Test]
        public async Task UpdatePropertyImageAsync_ShouldUpdateImage_WhenValidData()
        {
            // Arrange
            var imageId = 1;
            var updateDto = new UpdatePropertyImageDto
            {
                File = "updated.jpg",
                Enabled = false
            };
            var existingImage = new PropertyImage
            {
                IdPropertyImage = imageId,
                File = "original.jpg",
                Enabled = true,
                IdProperty = 1
            };
            var updatedImageDto = new PropertyImageDto
            {
                IdPropertyImage = imageId,
                File = "updated.jpg",
                Enabled = false
            };

            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync(existingImage);
            _mockMapper.Setup(x => x.Map(updateDto, existingImage));
            _mockMapper.Setup(x => x.Map<PropertyImageDto>(existingImage)).Returns(updatedImageDto);

            // Act
            var result = await _propertyImageService.UpdatePropertyImageAsync(imageId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedImageDto);
            _mockPropertyImageRepository.Verify(x => x.UpdateAsync(existingImage), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdatePropertyImageAsync_ShouldReturnNull_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            var updateDto = new UpdatePropertyImageDto
            {
                File = "updated.jpg",
                Enabled = false
            };

            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync((PropertyImage?)null);

            // Act
            var result = await _propertyImageService.UpdatePropertyImageAsync(imageId, updateDto);

            // Assert
            result.Should().BeNull();
            _mockPropertyImageRepository.Verify(x => x.UpdateAsync(It.IsAny<PropertyImage>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region DeletePropertyImageAsync Tests

        [Test]
        public async Task DeletePropertyImageAsync_ShouldDeleteImage_WhenImageExists()
        {
            // Arrange
            var imageId = 1;
            var existingImage = new PropertyImage
            {
                IdPropertyImage = imageId,
                File = "test.jpg",
                Enabled = true,
                IdProperty = 1
            };

            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync(existingImage);

            // Act
            var result = await _propertyImageService.DeletePropertyImageAsync(imageId);

            // Assert
            result.Should().BeTrue();
            _mockPropertyImageRepository.Verify(x => x.DeleteAsync(existingImage), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeletePropertyImageAsync_ShouldReturnFalse_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = 999;
            _mockPropertyImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync((PropertyImage?)null);

            // Act
            var result = await _propertyImageService.DeletePropertyImageAsync(imageId);

            // Assert
            result.Should().BeFalse();
            _mockPropertyImageRepository.Verify(x => x.DeleteAsync(It.IsAny<PropertyImage>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        #endregion
    }
}