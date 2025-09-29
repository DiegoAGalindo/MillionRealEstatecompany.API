using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.DTOs;
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

        #region SearchProperties Tests

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldFilterByPriceRange()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Property 1", Address = "Address 1", Price = 100000, CodeInternal = "PROP001", Year = 2020, IdOwner = owner.IdOwner },
                new() { Name = "Property 2", Address = "Address 2", Price = 250000, CodeInternal = "PROP002", Year = 2021, IdOwner = owner.IdOwner },
                new() { Name = "Property 3", Address = "Address 3", Price = 400000, CodeInternal = "PROP003", Year = 2022, IdOwner = owner.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter { MinPrice = 200000, MaxPrice = 300000 };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Property 2");
            result.First().Price.Should().Be(250000);
        }

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldFilterByYearRange()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Property 1", Address = "Address 1", Price = 100000, CodeInternal = "PROP001", Year = 2018, IdOwner = owner.IdOwner },
                new() { Name = "Property 2", Address = "Address 2", Price = 200000, CodeInternal = "PROP002", Year = 2020, IdOwner = owner.IdOwner },
                new() { Name = "Property 3", Address = "Address 3", Price = 300000, CodeInternal = "PROP003", Year = 2022, IdOwner = owner.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter { MinYear = 2019, MaxYear = 2021 };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Property 2");
            result.First().Year.Should().Be(2020);
        }

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldFilterByOwner()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Property 1", Address = "Address 1", Price = 100000, CodeInternal = "PROP001", Year = 2020, IdOwner = owner1.IdOwner },
                new() { Name = "Property 2", Address = "Address 2", Price = 200000, CodeInternal = "PROP002", Year = 2021, IdOwner = owner2.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter { OwnerId = owner1.IdOwner };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Property 1");
            result.First().IdOwner.Should().Be(owner1.IdOwner);
        }

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldFilterByCity()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Property 1", Address = "123 Main St, Bogotá", Price = 100000, CodeInternal = "PROP001", Year = 2020, IdOwner = owner.IdOwner },
                new() { Name = "Property 2", Address = "456 Oak Ave, Medellín", Price = 200000, CodeInternal = "PROP002", Year = 2021, IdOwner = owner.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter { City = "Bogotá" };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Property 1");
            result.First().Address.Should().Contain("Bogotá");
        }

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldFilterByName()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Apartamento Zona Rosa", Address = "Address 1", Price = 100000, CodeInternal = "PROP001", Year = 2020, IdOwner = owner.IdOwner },
                new() { Name = "Casa Campestre", Address = "Address 2", Price = 200000, CodeInternal = "PROP002", Year = 2021, IdOwner = owner.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter { Name = "Apartamento" };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Apartamento Zona Rosa");
        }

        [Test]
        public async Task PropertyRepository_SearchPropertiesAsync_ShouldCombineMultipleFilters()
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
            await _context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new() { Name = "Property 1", Address = "123 Main St, Bogotá", Price = 250000, CodeInternal = "PROP001", Year = 2020, IdOwner = owner.IdOwner },
                new() { Name = "Property 2", Address = "456 Oak Ave, Bogotá", Price = 350000, CodeInternal = "PROP002", Year = 2019, IdOwner = owner.IdOwner },
                new() { Name = "Property 3", Address = "789 Pine St, Medellín", Price = 300000, CodeInternal = "PROP003", Year = 2020, IdOwner = owner.IdOwner }
            };

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var repository = new PropertyRepository(_context);
            var filter = new PropertySearchFilter 
            { 
                MinPrice = 200000, 
                MaxPrice = 300000, 
                MinYear = 2020, 
                City = "Bogotá" 
            };

            // Act
            var result = await repository.SearchPropertiesAsync(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Property 1");
        }

        #endregion
    }
}