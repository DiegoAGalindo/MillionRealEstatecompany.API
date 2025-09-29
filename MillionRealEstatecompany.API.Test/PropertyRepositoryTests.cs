using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para PropertyRepository
    /// Testing de la capa de datos siguiendo los principios de TDD de Kent Beck
    /// </summary>
    [TestFixture]
    public class PropertyRepositoryTests
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

        #region PropertyRepository Tests

        [Test]
        public async Task PropertyRepository_GetPropertiesWithOwnerAsync_ShouldReturnPropertiesWithOwnerInfo()
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

            var repository = new PropertyRepository(_context);

            // Act
            var properties = await repository.GetPropertiesWithOwnerAsync();

            // Assert
            properties.Should().NotBeNull();
            properties.Should().HaveCount(1);
            var firstProperty = properties.First();
            firstProperty.Owner.Should().NotBeNull();
            firstProperty.Owner.Name.Should().Be("John Doe");
        }

        [Test]
        public async Task PropertyRepository_CodeInternalExistsAsync_ShouldReturnTrue_WhenCodeExists()
        {
            // Arrange
            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "UNIQUE_CODE",
                Year = 2023,
                IdOwner = 1
            };
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);

            // Act
            var exists = await repository.CodeInternalExistsAsync("UNIQUE_CODE");

            // Assert
            exists.Should().BeTrue();
        }

        [Test]
        public async Task PropertyRepository_CodeInternalExistsAsync_ShouldReturnFalse_WhenCodeDoesNotExist()
        {
            // Arrange
            var repository = new PropertyRepository(_context);

            // Act
            var exists = await repository.CodeInternalExistsAsync("NONEXISTENT_CODE");

            // Assert
            exists.Should().BeFalse();
        }

        [Test]
        public async Task PropertyRepository_CodeInternalExistsAsync_ShouldExcludeSpecifiedId()
        {
            // Arrange
            var property = new Property
            {
                Name = "Test Property",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "UNIQUE_CODE",
                Year = 2023,
                IdOwner = 1
            };
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);

            // Act
            var exists = await repository.CodeInternalExistsAsync("UNIQUE_CODE", property.IdProperty);

            // Assert
            exists.Should().BeFalse(); // Should return false because we're excluding this property's ID
        }

        [Test]
        public async Task PropertyRepository_GetPropertiesByOwnerAsync_ShouldReturnPropertiesForSpecificOwner()
        {
            // Arrange
            var owner1 = new Owner
            {
                Name = "John Doe",
                Address = "123 Main St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "12345678",
                Email = "john@example.com"
            };

            var owner2 = new Owner
            {
                Name = "Jane Smith",
                Address = "456 Oak Ave",
                Birthday = new DateOnly(1990, 5, 15),
                DocumentNumber = "87654321",
                Email = "jane@example.com"
            };

            await _context.Owners.AddRangeAsync(owner1, owner2);

            var property1 = new Property
            {
                Name = "Property 1",
                Address = "123 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner1
            };

            var property2 = new Property
            {
                Name = "Property 2",
                Address = "456 Property Ave",
                Price = 200000,
                CodeInternal = "PROP002",
                Year = 2023,
                Owner = owner1
            };

            var property3 = new Property
            {
                Name = "Property 3",
                Address = "789 Property Blvd",
                Price = 300000,
                CodeInternal = "PROP003",
                Year = 2023,
                Owner = owner2
            };

            await _context.Properties.AddRangeAsync(property1, property2, property3);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);

            // Act
            var propertiesForOwner1 = await repository.GetPropertiesByOwnerAsync(owner1.IdOwner);

            // Assert
            propertiesForOwner1.Should().NotBeNull();
            propertiesForOwner1.Should().HaveCount(2);
            propertiesForOwner1.Should().AllSatisfy(p => p.IdOwner.Should().Be(owner1.IdOwner));
        }

        #endregion
    }
}