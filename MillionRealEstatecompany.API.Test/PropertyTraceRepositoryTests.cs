using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para PropertyTraceRepository
    /// Testing de la capa de datos siguiendo los principios de TDD de Kent Beck
    /// </summary>
    [TestFixture]
    public class PropertyTraceRepositoryTests
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

        #region PropertyTraceRepository Tests

        [Test]
        public async Task PropertyTraceRepository_GetTracesByPropertyAsync_ShouldReturnTracesOrderedByDateSale()
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

            var trace1 = new PropertyTrace
            {
                DateSale = new DateTime(2023, 1, 15),
                Name = "Initial Sale",
                Value = 90000,
                Tax = 5000,
                Property = property
            };
            var trace2 = new PropertyTrace
            {
                DateSale = new DateTime(2023, 6, 20),
                Name = "Second Sale",
                Value = 95000,
                Tax = 5500,
                Property = property
            };
            var trace3 = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 10),
                Name = "Latest Sale",
                Value = 100000,
                Tax = 6000,
                Property = property
            };
            await _context.PropertyTraces.AddRangeAsync(trace1, trace2, trace3);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            var traces = await repository.GetTracesByPropertyAsync(property.IdProperty);

            // Assert
            traces.Should().NotBeNull();
            traces.Should().HaveCount(3);
            
            // Should be ordered by DateSale descending (most recent first)
            var tracesList = traces.ToList();
            tracesList[0].Name.Should().Be("Latest Sale");
            tracesList[1].Name.Should().Be("Second Sale");
            tracesList[2].Name.Should().Be("Initial Sale");
        }

        [Test]
        public async Task PropertyTraceRepository_GetTracesByPropertyAsync_ShouldReturnEmptyList_WhenNoTracesExist()
        {
            // Arrange
            var repository = new PropertyTraceRepository(_context);

            // Act
            var traces = await repository.GetTracesByPropertyAsync(999);

            // Assert
            traces.Should().NotBeNull();
            traces.Should().BeEmpty();
        }

        [Test]
        public async Task PropertyTraceRepository_GetLatestTraceByPropertyAsync_ShouldReturnMostRecentTrace()
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

            var oldTrace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 1, 15),
                Name = "Old Sale",
                Value = 90000,
                Tax = 5000,
                Property = property
            };
            var latestTrace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 10),
                Name = "Latest Sale",
                Value = 100000,
                Tax = 6000,
                Property = property
            };
            await _context.PropertyTraces.AddRangeAsync(oldTrace, latestTrace);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            var trace = await repository.GetLatestTraceByPropertyAsync(property.IdProperty);

            // Assert
            trace.Should().NotBeNull();
            trace!.Name.Should().Be("Latest Sale");
            trace.DateSale.Should().Be(new DateTime(2023, 12, 10));
        }

        [Test]
        public async Task PropertyTraceRepository_GetLatestTraceByPropertyAsync_ShouldReturnNull_WhenNoTracesExist()
        {
            // Arrange
            var repository = new PropertyTraceRepository(_context);

            // Act
            var trace = await repository.GetLatestTraceByPropertyAsync(999);

            // Assert
            trace.Should().BeNull();
        }

        [Test]
        public async Task PropertyTraceRepository_GetTracesByPropertyAsync_ShouldReturnOnlyTracesForSpecifiedProperty()
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

            var property1 = new Property
            {
                Name = "Property 1",
                Address = "456 Property St",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                Owner = owner
            };
            var property2 = new Property
            {
                Name = "Property 2",
                Address = "789 Property Ave",
                Price = 200000,
                CodeInternal = "PROP002",
                Year = 2023,
                Owner = owner
            };
            await _context.Properties.AddRangeAsync(property1, property2);

            var trace1 = new PropertyTrace
            {
                DateSale = new DateTime(2023, 1, 15),
                Name = "Property 1 Sale",
                Value = 90000,
                Tax = 5000,
                Property = property1
            };
            var trace2 = new PropertyTrace
            {
                DateSale = new DateTime(2023, 2, 20),
                Name = "Property 2 Sale",
                Value = 180000,
                Tax = 10000,
                Property = property2
            };
            await _context.PropertyTraces.AddRangeAsync(trace1, trace2);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            var tracesForProperty1 = await repository.GetTracesByPropertyAsync(property1.IdProperty);

            // Assert
            tracesForProperty1.Should().NotBeNull();
            tracesForProperty1.Should().HaveCount(1);
            tracesForProperty1.First().Name.Should().Be("Property 1 Sale");
        }

        #endregion

        #region Generic Repository Tests

        [Test]
        public async Task PropertyTraceRepository_AddAsync_ShouldAddEntity()
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

            var repository = new PropertyTraceRepository(_context);
            var trace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Christmas Sale",
                Value = 105000,
                Tax = 6500,
                IdProperty = property.IdProperty
            };

            // Act
            var addedTrace = await repository.AddAsync(trace);
            await _context.SaveChangesAsync();

            // Assert
            addedTrace.Should().NotBeNull();
            addedTrace.IdPropertyTrace.Should().BeGreaterThan(0);
            
            var foundTrace = await _context.PropertyTraces.FindAsync(addedTrace.IdPropertyTrace);
            foundTrace.Should().NotBeNull();
            foundTrace!.Name.Should().Be("Christmas Sale");
        }

        [Test]
        public async Task PropertyTraceRepository_GetByIdAsync_ShouldReturnTrace_WhenTraceExists()
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

            var trace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Test Sale",
                Value = 105000,
                Tax = 6500,
                Property = property
            };
            await _context.PropertyTraces.AddAsync(trace);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            var foundTrace = await repository.GetByIdAsync(trace.IdPropertyTrace);

            // Assert
            foundTrace.Should().NotBeNull();
            foundTrace!.Name.Should().Be("Test Sale");
        }

        [Test]
        public async Task PropertyTraceRepository_GetByIdAsync_ShouldReturnNull_WhenTraceDoesNotExist()
        {
            // Arrange
            var repository = new PropertyTraceRepository(_context);

            // Act
            var foundTrace = await repository.GetByIdAsync(999);

            // Assert
            foundTrace.Should().BeNull();
        }

        [Test]
        public async Task PropertyTraceRepository_ExistsAsync_ShouldReturnTrue_WhenEntityExists()
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

            var trace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Test Sale",
                Value = 105000,
                Tax = 6500,
                Property = property
            };
            await _context.PropertyTraces.AddAsync(trace);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(trace.IdPropertyTrace);

            // Assert
            exists.Should().BeTrue();
        }

        [Test]
        public async Task PropertyTraceRepository_ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var repository = new PropertyTraceRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(999);

            // Assert
            exists.Should().BeFalse();
        }

        [Test]
        public async Task PropertyTraceRepository_UpdateAsync_ShouldUpdateEntity()
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

            var trace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Original Sale",
                Value = 105000,
                Tax = 6500,
                Property = property
            };
            await _context.PropertyTraces.AddAsync(trace);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            trace.Name = "Updated Sale";
            await repository.UpdateAsync(trace);
            await _context.SaveChangesAsync();

            // Assert
            var updatedTrace = await _context.PropertyTraces.FindAsync(trace.IdPropertyTrace);
            updatedTrace.Should().NotBeNull();
            updatedTrace!.Name.Should().Be("Updated Sale");
        }

        [Test]
        public async Task PropertyTraceRepository_DeleteAsync_ShouldRemoveEntity()
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

            var trace = new PropertyTrace
            {
                DateSale = new DateTime(2023, 12, 25),
                Name = "Test Sale",
                Value = 105000,
                Tax = 6500,
                Property = property
            };
            await _context.PropertyTraces.AddAsync(trace);
            await _context.SaveChangesAsync();

            var repository = new PropertyTraceRepository(_context);

            // Act
            await repository.DeleteAsync(trace);
            await _context.SaveChangesAsync();

            // Assert
            var deletedTrace = await _context.PropertyTraces.FindAsync(trace.IdPropertyTrace);
            deletedTrace.Should().BeNull();
        }

        #endregion
    }
}
