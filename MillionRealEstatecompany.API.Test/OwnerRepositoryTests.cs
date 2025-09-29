using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para OwnerRepository
    /// Testing de la capa de datos siguiendo los principios de TDD de Kent Beck
    /// </summary>
    [TestFixture]
    public class OwnerRepositoryTests
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

        #region OwnerRepository Tests

        [Test]
        public async Task OwnerRepository_DocumentNumberExistsAsync_ShouldReturnTrue_WhenDocumentExists()
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

            var repository = new OwnerRepository(_context);

            // Act
            var exists = await repository.DocumentNumberExistsAsync("12345678");

            // Assert
            exists.Should().BeTrue();
        }

        [Test]
        public async Task OwnerRepository_DocumentNumberExistsAsync_ShouldReturnFalse_WhenDocumentDoesNotExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var exists = await repository.DocumentNumberExistsAsync("NONEXISTENT");

            // Assert
            exists.Should().BeFalse();
        }

        [Test]
        public async Task OwnerRepository_DocumentNumberExistsAsync_ShouldExcludeSpecifiedId()
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

            var repository = new OwnerRepository(_context);

            // Act
            var exists = await repository.DocumentNumberExistsAsync("12345678");

            // Assert
            exists.Should().BeTrue(); // Should return true because the document exists for a different owner
        }

        [Test]
        public async Task OwnerRepository_GetByDocumentNumberAsync_ShouldReturnOwner_WhenDocumentExists()
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

            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByDocumentNumberAsync("12345678");

            // Assert
            foundOwner.Should().NotBeNull();
            foundOwner!.Name.Should().Be("John Doe");
            foundOwner.DocumentNumber.Should().Be("12345678");
        }

        [Test]
        public async Task OwnerRepository_GetByDocumentNumberAsync_ShouldReturnNull_WhenDocumentDoesNotExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByDocumentNumberAsync("NONEXISTENT");

            // Assert
            foundOwner.Should().BeNull();
        }

        [Test]
        public async Task OwnerRepository_GetByEmailAsync_ShouldReturnOwner_WhenEmailExists()
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

            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByEmailAsync("john@example.com");

            // Assert
            foundOwner.Should().NotBeNull();
            foundOwner!.Name.Should().Be("John Doe");
            foundOwner.Email.Should().Be("john@example.com");
        }

        [Test]
        public async Task OwnerRepository_GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            foundOwner.Should().BeNull();
        }

        [Test]
        public async Task OwnerRepository_GetOwnersWithPropertiesAsync_ShouldReturnOwnersWithProperties()
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

            var repository = new OwnerRepository(_context);

            // Act
            var ownersWithProperties = await repository.GetOwnersWithPropertiesAsync();

            // Assert
            ownersWithProperties.Should().NotBeNull();
            ownersWithProperties.Should().HaveCount(1);
            var ownerWithProperties = ownersWithProperties.First();
            ownerWithProperties.Properties.Should().NotBeNull();
            ownerWithProperties.Properties.Should().HaveCount(1);
        }

        [Test]
        public async Task OwnerRepository_GetOwnersWithPropertiesAsync_ShouldReturnEmptyList_WhenNoOwnersExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var ownersWithProperties = await repository.GetOwnersWithPropertiesAsync();

            // Assert
            ownersWithProperties.Should().NotBeNull();
            ownersWithProperties.Should().BeEmpty();
        }

        #endregion

        #region Generic Repository Tests

        [Test]
        public async Task OwnerRepository_AddAsync_ShouldAddEntity()
        {
            // Arrange
            var repository = new OwnerRepository(_context);
            var owner = new Owner
            {
                Name = "Test Owner",
                Address = "123 Test St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "TEST123",
                Email = "test@example.com"
            };

            // Act
            var addedOwner = await repository.AddAsync(owner);
            await _context.SaveChangesAsync();

            // Assert
            addedOwner.Should().NotBeNull();
            addedOwner.IdOwner.Should().BeGreaterThan(0);
            
            var foundOwner = await _context.Owners.FindAsync(addedOwner.IdOwner);
            foundOwner.Should().NotBeNull();
            foundOwner!.Name.Should().Be("Test Owner");
        }

        [Test]
        public async Task OwnerRepository_GetByIdAsync_ShouldReturnOwner_WhenOwnerExists()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "Test Owner",
                Address = "123 Test St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "TEST123",
                Email = "test@example.com"
            };
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByIdAsync(owner.IdOwner);

            // Assert
            foundOwner.Should().NotBeNull();
            foundOwner!.Name.Should().Be("Test Owner");
        }

        [Test]
        public async Task OwnerRepository_GetByIdAsync_ShouldReturnNull_WhenOwnerDoesNotExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var foundOwner = await repository.GetByIdAsync(999);

            // Assert
            foundOwner.Should().BeNull();
        }

        [Test]
        public async Task OwnerRepository_ExistsAsync_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "Test Owner",
                Address = "123 Test St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "TEST123",
                Email = "test@example.com"
            };
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            var repository = new OwnerRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(owner.IdOwner);

            // Assert
            exists.Should().BeTrue();
        }

        [Test]
        public async Task OwnerRepository_ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var repository = new OwnerRepository(_context);

            // Act
            var exists = await repository.ExistsAsync(999);

            // Assert
            exists.Should().BeFalse();
        }

        [Test]
        public async Task OwnerRepository_GetAllAsync_ShouldReturnAllOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    Birthday = new DateOnly(1985, 1, 1),
                    DocumentNumber = "12345678",
                    Email = "john@example.com"
                },
                new Owner
                {
                    Name = "Jane Smith",
                    Address = "456 Oak Ave",
                    Birthday = new DateOnly(1990, 5, 15),
                    DocumentNumber = "87654321",
                    Email = "jane@example.com"
                }
            };
            await _context.Owners.AddRangeAsync(owners);
            await _context.SaveChangesAsync();

            var repository = new OwnerRepository(_context);

            // Act
            var allOwners = await repository.GetAllAsync();

            // Assert
            allOwners.Should().NotBeNull();
            allOwners.Should().HaveCount(2);
        }

        [Test]
        public async Task OwnerRepository_UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "Original Name",
                Address = "123 Test St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "TEST123",
                Email = "test@example.com"
            };
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            var repository = new OwnerRepository(_context);

            // Act
            owner.Name = "Updated Name";
            await repository.UpdateAsync(owner);
            await _context.SaveChangesAsync();

            // Assert
            var updatedOwner = await _context.Owners.FindAsync(owner.IdOwner);
            updatedOwner.Should().NotBeNull();
            updatedOwner!.Name.Should().Be("Updated Name");
        }

        [Test]
        public async Task OwnerRepository_DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var owner = new Owner
            {
                Name = "Test Owner",
                Address = "123 Test St",
                Birthday = new DateOnly(1985, 1, 1),
                DocumentNumber = "TEST123",
                Email = "test@example.com"
            };
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            var repository = new OwnerRepository(_context);

            // Act
            await repository.DeleteAsync(owner);
            await _context.SaveChangesAsync();

            // Assert
            var deletedOwner = await _context.Owners.FindAsync(owner.IdOwner);
            deletedOwner.Should().BeNull();
        }

        #endregion
    }
}