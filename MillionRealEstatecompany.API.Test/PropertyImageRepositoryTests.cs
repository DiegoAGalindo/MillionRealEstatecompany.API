using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para PropertyImageRepository
    /// Testing de la capa de datos siguiendo los principios de TDD de Kent Beck
    /// </summary>
    [TestFixture]
    public class PropertyImageRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region PropertyImageRepository Tests

        [Test]
        public async Task PropertyImageRepository_GetImagesByPropertyAsync_ShouldReturnImagesForProperty()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var image1 = new PropertyImage
            {
                File = "image1.jpg",
                Enabled = true,
                Property = property
            };
            var image2 = new PropertyImage
            {
                File = "image2.jpg",
                Enabled = false,
                Property = property
            };
            await _context.PropertyImages.AddRangeAsync(image1, image2);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            var images = await repository.GetImagesByPropertyAsync(property.IdProperty);

            // Assert
            images.Should().NotBeNull();
            images.Should().HaveCount(2);
            images.Should().Contain(i => i.File == "image1.jpg");
            images.Should().Contain(i => i.File == "image2.jpg");
        }

        [Test]
        public async Task PropertyImageRepository_GetImagesByPropertyAsync_ShouldReturnEmptyList_WhenNoImagesExist()
        {
            // Arrange
            var repository = new PropertyImageRepository(_context);

            // Act
            var images = await repository.GetImagesByPropertyAsync(999);

            // Assert
            images.Should().NotBeNull();
            images.Should().BeEmpty();
        }

        [Test]
        public async Task PropertyImageRepository_GetEnabledImagesByPropertyAsync_ShouldReturnOnlyEnabledImages()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var enabledImage = new PropertyImage
            {
                File = "enabled.jpg",
                Enabled = true,
                Property = property
            };
            var disabledImage = new PropertyImage
            {
                File = "disabled.jpg",
                Enabled = false,
                Property = property
            };
            await _context.PropertyImages.AddRangeAsync(enabledImage, disabledImage);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            var enabledImages = await repository.GetEnabledImagesByPropertyAsync(property.IdProperty);

            // Assert
            enabledImages.Should().NotBeNull();
            enabledImages.Should().HaveCount(1);
            enabledImages.First().File.Should().Be("enabled.jpg");
            enabledImages.First().Enabled.Should().BeTrue();
        }

        [Test]
        public async Task PropertyImageRepository_GetEnabledImagesByPropertyAsync_ShouldReturnEmptyList_WhenNoEnabledImagesExist()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var disabledImage = new PropertyImage
            {
                File = "disabled.jpg",
                Enabled = false,
                Property = property
            };
            await _context.PropertyImages.AddAsync(disabledImage);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            var enabledImages = await repository.GetEnabledImagesByPropertyAsync(property.IdProperty);

            // Assert
            enabledImages.Should().NotBeNull();
            enabledImages.Should().BeEmpty();
        }

        #endregion

        #region Generic Repository Tests

        [Test]
        public async Task PropertyImageRepository_AddAsync_ShouldAddEntity()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);
            var image = new PropertyImage
            {
                File = "test.jpg",
                Enabled = true,
                IdProperty = property.IdProperty
            };

            // Act
            var addedImage = await repository.AddAsync(image);
            await _context.SaveChangesAsync();

            // Assert
            addedImage.Should().NotBeNull();
            addedImage.IdPropertyImage.Should().BeGreaterThan(0);
            
            var foundImage = await _context.PropertyImages.FindAsync(addedImage.IdPropertyImage);
            foundImage.Should().NotBeNull();
            foundImage!.File.Should().Be("test.jpg");
        }

        [Test]
        public async Task PropertyImageRepository_GetByIdAsync_ShouldReturnImage_WhenImageExists()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var image = new PropertyImage
            {
                File = "test.jpg",
                Enabled = true,
                Property = property
            };
            await _context.PropertyImages.AddAsync(image);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            var foundImage = await repository.GetByIdAsync(image.IdPropertyImage);

            // Assert
            foundImage.Should().NotBeNull();
            foundImage!.File.Should().Be("test.jpg");
        }

        [Test]
        public async Task PropertyImageRepository_GetByIdAsync_ShouldReturnNull_WhenImageDoesNotExist()
        {
            // Arrange
            var repository = new PropertyImageRepository(_context);

            // Act
            var foundImage = await repository.GetByIdAsync(999);

            // Assert
            foundImage.Should().BeNull();
        }

        [Test]
        public async Task PropertyImageRepository_ExistsAsync_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var image = new PropertyImage
            {
                File = "test.jpg",
                Enabled = true,
                Property = property
            };
            await _context.PropertyImages.AddAsync(image);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(image.IdPropertyImage);

            // Assert
            exists.Should().BeTrue();
        }

        [Test]
        public async Task PropertyImageRepository_ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var repository = new PropertyImageRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(999);

            // Assert
            exists.Should().BeFalse();
        }

        [Test]
        public async Task PropertyImageRepository_UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var image = new PropertyImage
            {
                File = "original.jpg",
                Enabled = true,
                Property = property
            };
            await _context.PropertyImages.AddAsync(image);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            image.File = "updated.jpg";
            await repository.UpdateAsync(image);
            await _context.SaveChangesAsync();

            // Assert
            var updatedImage = await _context.PropertyImages.FindAsync(image.IdPropertyImage);
            updatedImage.Should().NotBeNull();
            updatedImage!.File.Should().Be("updated.jpg");
        }

        [Test]
        public async Task PropertyImageRepository_DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };
            await _context.Owners.AddAsync(owner);

            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddAsync(property);

            var image = new PropertyImage
            {
                File = "test.jpg",
                Enabled = true,
                Property = property
            };
            await _context.PropertyImages.AddAsync(image);
            await _context.SaveChangesAsync();

            var repository = new PropertyImageRepository(_context);

            // Act
            await repository.DeleteAsync(image);
            await _context.SaveChangesAsync();

            // Assert
            var deletedImage = await _context.PropertyImages.FindAsync(image.IdPropertyImage);
            deletedImage.Should().BeNull();
        }

        #endregion
    }
}