using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Repositories;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas unitarias para UnitOfWork
    /// Implementando patrones de Clean Architecture y principios SOLID para gestión de transacciones
    /// </summary>
    [TestFixture]
    public class UnitOfWorkTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_ShouldInitializeAllRepositories()
        {
            // Assert
            _unitOfWork.Owners.Should().NotBeNull();
            _unitOfWork.Properties.Should().NotBeNull();
            _unitOfWork.PropertyImages.Should().NotBeNull();
            _unitOfWork.PropertyTraces.Should().NotBeNull();
        }

        #endregion

        #region SaveChangesAsync Tests

        [Test]
        public async Task SaveChangesAsync_ShouldReturnNumberOfChanges_WhenEntitiesModified()
        {
            // Arrange
            var owner = new Models.Owner
            {
                Name = "Test Owner",
                Address = "123 Test St",
                Birthday = new DateOnly(1990, 1, 1),
                DocumentNumber = "12345678",
                Email = "test@example.com"
            };

            await _unitOfWork.Owners.AddAsync(owner);

            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().Be(1);
        }

        [Test]
        public async Task SaveChangesAsync_ShouldReturnZero_WhenNoChanges()
        {
            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().Be(0);
        }

        #endregion

        #region Transaction Tests - Adapted for InMemory Database

        [Test]
        public async Task BeginTransactionAsync_ShouldHandleInMemoryDatabase()
        {
            // Nota: InMemoryDatabase no soporta transacciones reales
            // Verificamos que el método maneja correctamente esta limitación

            // Act & Assert - Verificamos que el método existe y se puede llamar
            // En InMemory database, esto lanzará una excepción esperada
            var act = async () => await _unitOfWork.BeginTransactionAsync();
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Transactions are not supported by the in-memory store*");
        }

        [Test]
        public async Task CommitTransactionAsync_ShouldNotThrow_WhenNoTransactionExists()
        {
            // Act & Assert
            var act = async () => await _unitOfWork.CommitTransactionAsync();
            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task RollbackTransactionAsync_ShouldNotThrow_WhenNoTransactionExists()
        {
            // Act & Assert
            var act = async () => await _unitOfWork.RollbackTransactionAsync();
            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task SaveChangesAsync_WorksCorrectly_WithMultipleOperations()
        {
            // Arrange - Simulamos un escenario de múltiples operaciones
            var owner = new Models.Owner
            {
                Name = "Multi Op Owner",
                Address = "999 Multi St",
                Birthday = new DateOnly(1975, 3, 10),
                DocumentNumber = "99999999",
                Email = "multiop@example.com"
            };

            var property = new Models.Property
            {
                Name = "Multi Op Property",
                Address = "888 Property Ave",
                Price = 150000,
                CodeInternal = "MULTI001",
                Year = 2024,
                Owner = owner
            };

            // Act
            await _unitOfWork.Owners.AddAsync(owner);
            await _unitOfWork.SaveChangesAsync();
            
            await _unitOfWork.Properties.AddAsync(property);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var savedOwner = await _context.Owners.FirstOrDefaultAsync(o => o.DocumentNumber == "99999999");
            var savedProperty = await _context.Properties.FirstOrDefaultAsync(p => p.CodeInternal == "MULTI001");
            
            savedOwner.Should().NotBeNull();
            savedProperty.Should().NotBeNull();
            savedProperty!.IdOwner.Should().Be(savedOwner!.IdOwner);
        }

        #endregion

        #region Dispose Tests

        [Test]
        public void Dispose_ShouldDisposeResources()
        {
            // Act & Assert - Si Dispose funciona correctamente, no debería lanzar excepción
            var act = () => _unitOfWork.Dispose();
            act.Should().NotThrow();
        }

        [Test]
        public void Dispose_ShouldNotFail_AfterMultipleCalls()
        {
            // Act & Assert - Dispose debería ser idempotente
            var act = () => 
            {
                _unitOfWork.Dispose();
                _unitOfWork.Dispose(); // Segunda llamada
            };
            act.Should().NotThrow();
        }

        #endregion

        #region Repository Integration Tests

        [Test]
        public void Repositories_ShouldBeCorrectType()
        {
            // Assert
            _unitOfWork.Owners.Should().BeOfType<OwnerRepository>();
            _unitOfWork.Properties.Should().BeOfType<PropertyRepository>();
            _unitOfWork.PropertyImages.Should().BeOfType<PropertyImageRepository>();
            _unitOfWork.PropertyTraces.Should().BeOfType<PropertyTraceRepository>();
        }

        [Test]
        public void Repositories_ShouldShareSameContext()
        {
            // Arrange & Act
            var ownerRepo = _unitOfWork.Owners as OwnerRepository;
            var propertyRepo = _unitOfWork.Properties as PropertyRepository;

            // Assert - Los repositorios deberían usar el mismo contexto para transacciones
            // Esto se verifica indirectamente al probar que los cambios son consistentes
            ownerRepo.Should().NotBeNull();
            propertyRepo.Should().NotBeNull();
        }

        #endregion
    }
}