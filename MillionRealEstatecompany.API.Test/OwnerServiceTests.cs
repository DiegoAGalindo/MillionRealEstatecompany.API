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
    /// Pruebas unitarias para OwnerService
    /// Implementando patrones de Clean Code y TDD
    /// </summary>
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private Mock<IMapper> _mockMapper;
        private OwnerService _ownerService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(x => x.Owners).Returns(_mockOwnerRepository.Object);
            _ownerService = new OwnerService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region GetAllOwnersAsync Tests

        [Test]
        public async Task GetAllOwnersAsync_ShouldReturnAllOwners_WhenOwnersExist()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner { IdOwner = 1, Name = "John Doe", DocumentNumber = "12345678" },
                new Owner { IdOwner = 2, Name = "Jane Smith", DocumentNumber = "87654321" }
            };
            var ownerDtos = new List<OwnerDto>
            {
                new OwnerDto { IdOwner = 1, Name = "John Doe", DocumentNumber = "12345678" },
                new OwnerDto { IdOwner = 2, Name = "Jane Smith", DocumentNumber = "87654321" }
            };

            _mockOwnerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(owners);
            _mockMapper.Setup(x => x.Map<IEnumerable<OwnerDto>>(owners)).Returns(ownerDtos);

            // Act
            var result = await _ownerService.GetAllOwnersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(ownerDtos);
        }

        [Test]
        public async Task GetAllOwnersAsync_ShouldReturnEmptyList_WhenNoOwnersExist()
        {
            // Arrange
            var owners = new List<Owner>();
            var ownerDtos = new List<OwnerDto>();

            _mockOwnerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(owners);
            _mockMapper.Setup(x => x.Map<IEnumerable<OwnerDto>>(owners)).Returns(ownerDtos);

            // Act
            var result = await _ownerService.GetAllOwnersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetOwnerByIdAsync Tests

        [Test]
        public async Task GetOwnerByIdAsync_ShouldReturnOwner_WhenOwnerExists()
        {
            // Arrange
            var ownerId = 1;
            var owner = new Owner { IdOwner = ownerId, Name = "John Doe", DocumentNumber = "12345678" };
            var ownerDto = new OwnerDto { IdOwner = ownerId, Name = "John Doe", DocumentNumber = "12345678" };

            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mockMapper.Setup(x => x.Map<OwnerDto>(owner)).Returns(ownerDto);

            // Act
            var result = await _ownerService.GetOwnerByIdAsync(ownerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(ownerDto);
        }

        [Test]
        public async Task GetOwnerByIdAsync_ShouldReturnNull_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync((Owner?)null);

            // Act
            var result = await _ownerService.GetOwnerByIdAsync(ownerId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetOwnerByDocumentNumberAsync Tests

        [Test]
        public async Task GetOwnerByDocumentNumberAsync_ShouldReturnOwner_WhenOwnerExists()
        {
            // Arrange
            var documentNumber = "12345678";
            var owner = new Owner { IdOwner = 1, Name = "John Doe", DocumentNumber = documentNumber };
            var ownerDto = new OwnerDto { IdOwner = 1, Name = "John Doe", DocumentNumber = documentNumber };

            _mockOwnerRepository.Setup(x => x.GetByDocumentNumberAsync(documentNumber)).ReturnsAsync(owner);
            _mockMapper.Setup(x => x.Map<OwnerDto>(owner)).Returns(ownerDto);

            // Act
            var result = await _ownerService.GetOwnerByDocumentNumberAsync(documentNumber);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(ownerDto);
        }

        [Test]
        public async Task GetOwnerByDocumentNumberAsync_ShouldReturnNull_WhenOwnerDoesNotExist()
        {
            // Arrange
            var documentNumber = "99999999";
            _mockOwnerRepository.Setup(x => x.GetByDocumentNumberAsync(documentNumber)).ReturnsAsync((Owner?)null);

            // Act
            var result = await _ownerService.GetOwnerByDocumentNumberAsync(documentNumber);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetOwnerByDocumentNumberAsync_ShouldThrowArgumentException_WhenDocumentNumberIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _ownerService.GetOwnerByDocumentNumberAsync(null!));
        }

        [Test]
        public void GetOwnerByDocumentNumberAsync_ShouldThrowArgumentException_WhenDocumentNumberIsEmpty()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _ownerService.GetOwnerByDocumentNumberAsync(string.Empty));
        }

        [Test]
        public void GetOwnerByDocumentNumberAsync_ShouldThrowArgumentException_WhenDocumentNumberIsWhitespace()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _ownerService.GetOwnerByDocumentNumberAsync("   "));
        }

        #endregion

        #region CreateOwnerAsync Tests

        [Test]
        public async Task CreateOwnerAsync_ShouldCreateOwner_WhenValidDataProvided()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                DocumentNumber = "12345678",
                Birthday = new DateOnly(1985, 5, 15),
                Address = "123 Main Street"
            };

            var owner = new Owner
            {
                IdOwner = 1,
                Name = createDto.Name,
                DocumentNumber = createDto.DocumentNumber,
                Birthday = createDto.Birthday.Value,
                Address = createDto.Address
            };

            var ownerDto = new OwnerDto
            {
                IdOwner = 1,
                Name = createDto.Name,
                DocumentNumber = createDto.DocumentNumber,
                Birthday = createDto.Birthday.Value,
                Address = createDto.Address
            };

            _mockOwnerRepository.Setup(x => x.DocumentNumberExistsAsync(createDto.DocumentNumber))
                .ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<Owner>(createDto)).Returns(owner);
            _mockOwnerRepository.Setup(x => x.AddAsync(It.IsAny<Owner>())).ReturnsAsync(owner);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(x => x.Map<OwnerDto>(owner)).Returns(ownerDto);

            // Act
            var result = await _ownerService.CreateOwnerAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(ownerDto);
            _mockOwnerRepository.Verify(x => x.AddAsync(It.IsAny<Owner>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateOwnerAsync_ShouldThrowArgumentException_WhenBirthdayIsNull()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                DocumentNumber = "12345678",
                Birthday = null, // Invalid
                Address = "123 Main Street"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _ownerService.CreateOwnerAsync(createDto));
        }

        [Test]
        public void CreateOwnerAsync_ShouldThrowArgumentException_WhenDocumentNumberExists()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                DocumentNumber = "12345678",
                Birthday = new DateOnly(1985, 5, 15),
                Address = "123 Main Street"
            };

            _mockOwnerRepository.Setup(x => x.DocumentNumberExistsAsync(createDto.DocumentNumber))
                .ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _ownerService.CreateOwnerAsync(createDto));
        }

        #endregion

        #region UpdateOwnerAsync Tests

        [Test]
        public async Task UpdateOwnerAsync_ShouldUpdateOwner_WhenValidDataProvided()
        {
            // Arrange
            var ownerId = 1;
            var updateDto = new UpdateOwnerDto
            {
                Name = "John Doe Updated",
                DocumentNumber = "87654321",
                Birthday = new DateOnly(1985, 5, 15),
                Address = "456 Updated Street"
            };

            var existingOwner = new Owner
            {
                IdOwner = ownerId,
                Name = "John Doe",
                DocumentNumber = "12345678",
                Birthday = new DateOnly(1985, 5, 15),
                Address = "123 Main Street"
            };

            var updatedOwnerDto = new OwnerDto
            {
                IdOwner = ownerId,
                Name = updateDto.Name,
                DocumentNumber = updateDto.DocumentNumber,
                Birthday = updateDto.Birthday,
                Address = updateDto.Address
            };

            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync(existingOwner);
            _mockOwnerRepository.Setup(x => x.DocumentNumberExistsAsync(updateDto.DocumentNumber))
                .ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map(updateDto, existingOwner));
            _mockOwnerRepository.Setup(x => x.UpdateAsync(existingOwner)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(x => x.Map<OwnerDto>(existingOwner)).Returns(updatedOwnerDto);

            // Act
            var result = await _ownerService.UpdateOwnerAsync(ownerId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedOwnerDto);
            _mockOwnerRepository.Verify(x => x.UpdateAsync(existingOwner), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateOwnerAsync_ShouldReturnNull_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            var updateDto = new UpdateOwnerDto
            {
                Name = "John Doe Updated",
                DocumentNumber = "87654321",
                Birthday = new DateOnly(1985, 5, 15),
                Address = "456 Updated Street"
            };

            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync((Owner?)null);

            // Act
            var result = await _ownerService.UpdateOwnerAsync(ownerId, updateDto);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region DeleteOwnerAsync Tests

        [Test]
        public async Task DeleteOwnerAsync_ShouldReturnTrue_WhenOwnerIsDeleted()
        {
            // Arrange
            var ownerId = 1;
            var owner = new Owner { IdOwner = ownerId, Name = "John Doe" };

            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mockOwnerRepository.Setup(x => x.DeleteAsync(owner)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _ownerService.DeleteOwnerAsync(ownerId);

            // Assert
            result.Should().BeTrue();
            _mockOwnerRepository.Verify(x => x.DeleteAsync(owner), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteOwnerAsync_ShouldReturnFalse_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            _mockOwnerRepository.Setup(x => x.GetByIdAsync(ownerId)).ReturnsAsync((Owner?)null);

            // Act
            var result = await _ownerService.DeleteOwnerAsync(ownerId);

            // Assert
            result.Should().BeFalse();
            _mockOwnerRepository.Verify(x => x.DeleteAsync(It.IsAny<Owner>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region OwnerExistsAsync Tests

        [Test]
        public async Task OwnerExistsAsync_ShouldReturnTrue_WhenOwnerExists()
        {
            // Arrange
            var ownerId = 1;
            _mockOwnerRepository.Setup(x => x.ExistsAsync(ownerId)).ReturnsAsync(true);

            // Act
            var result = await _ownerService.OwnerExistsAsync(ownerId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task OwnerExistsAsync_ShouldReturnFalse_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = 999;
            _mockOwnerRepository.Setup(x => x.ExistsAsync(ownerId)).ReturnsAsync(false);

            // Act
            var result = await _ownerService.OwnerExistsAsync(ownerId);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}