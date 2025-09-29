using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace MillionRealEstatecompany.API.Test
{
    /// <summary>
    /// Pruebas de integración para la API
    /// Implementando testing de extremo a extremo siguiendo las mejores prácticas de Jez Humble
    /// </summary>
    [TestFixture]
    public class IntegrationTests : IDisposable
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private ApplicationDbContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace the database with in-memory database for testing
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase("TestDb"));
                    });
                    
                    // Configure testing environment
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        context.HostingEnvironment.EnvironmentName = "Testing";
                    });
                });

            _client = _factory.CreateClient();

            // Get the database context and seed test data
            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _context.Database.EnsureCreated();
            SeedTestData();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context?.Dispose();
            _client?.Dispose();
            _factory?.Dispose();
        }

        public void Dispose()
        {
            OneTimeTearDown();
        }

        private void SeedTestData()
        {
            // Clear existing data
            _context.PropertyTraces.RemoveRange(_context.PropertyTraces);
            _context.PropertyImages.RemoveRange(_context.PropertyImages);
            _context.Properties.RemoveRange(_context.Properties);
            _context.Owners.RemoveRange(_context.Owners);

            // Add test owners
            var owners = new List<Owner>
            {
                new Owner
                {
                    IdOwner = 1,
                    Name = "John Doe",
                    Address = "123 Main Street",
                    Birthday = new DateOnly(1985, 5, 15),
                    DocumentNumber = "12345678",
                    Email = "john@example.com"
                },
                new Owner
                {
                    IdOwner = 2,
                    Name = "Jane Smith",
                    Address = "456 Oak Avenue",
                    Birthday = new DateOnly(1990, 10, 20),
                    DocumentNumber = "87654321",
                    Email = "jane@example.com"
                }
            };
            _context.Owners.AddRange(owners);

            // Add test properties
            var properties = new List<Property>
            {
                new Property
                {
                    IdProperty = 1,
                    Name = "Beautiful House",
                    Address = "123 Property Street",
                    Price = 250000,
                    CodeInternal = "HOUSE001",
                    Year = 2020,
                    IdOwner = 1
                },
                new Property
                {
                    IdProperty = 2,
                    Name = "Modern Apartment",
                    Address = "456 Apartment Avenue",
                    Price = 180000,
                    CodeInternal = "APT001",
                    Year = 2022,
                    IdOwner = 2
                }
            };
            _context.Properties.AddRange(properties);

            _context.SaveChanges();
        }

        #region Properties Integration Tests

        [Test]
        public async Task GetAllProperties_ShouldReturnAllProperties()
        {
            // Act
            var response = await _client.GetAsync("/api/properties");
            var properties = await response.Content.ReadFromJsonAsync<List<PropertyDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            properties.Should().NotBeNull();
            properties.Should().HaveCountGreaterOrEqualTo(2); // At least the seeded properties
            properties.Should().Contain(p => p.Name == "Beautiful House");
            properties.Should().Contain(p => p.Name == "Modern Apartment");
        }

        [Test]
        public async Task GetPropertyById_ShouldReturnProperty_WhenPropertyExists()
        {
            // Act
            var response = await _client.GetAsync("/api/properties/1");
            var property = await response.Content.ReadFromJsonAsync<PropertyDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            property.Should().NotBeNull();
            property!.IdProperty.Should().Be(1);
            property.Name.Should().Be("Beautiful House");
        }

        [Test]
        public async Task GetPropertyById_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/properties/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CreateProperty_ShouldCreateProperty_WhenValidDataProvided()
        {
            // Arrange
            var newProperty = new CreatePropertyDto
            {
                Name = "New Test Property",
                Address = "789 New Street",
                Price = 300000,
                CodeInternal = "NEW001",
                Year = 2023,
                IdOwner = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/properties", newProperty);
            var createdProperty = await response.Content.ReadFromJsonAsync<PropertyDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdProperty.Should().NotBeNull();
            createdProperty!.Name.Should().Be(newProperty.Name);
            createdProperty.Address.Should().Be(newProperty.Address);
            createdProperty.Price.Should().Be(newProperty.Price.Value);

            // Verify it was actually created in the database
            var getResponse = await _client.GetAsync($"/api/properties/{createdProperty.IdProperty}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task CreateProperty_ShouldReturnBadRequest_WhenOwnerDoesNotExist()
        {
            // Arrange
            var newProperty = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test Street",
                Price = 100000,
                CodeInternal = "TEST001",
                Year = 2023,
                IdOwner = 999 // Non-existent owner
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/properties", newProperty);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateProperty_ShouldUpdateProperty_WhenValidDataProvided()
        {
            // Arrange
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Beautiful House",
                Address = "123 Updated Property Street",
                Price = 275000,
                CodeInternal = "HOUSE001",
                Year = 2020,
                IdOwner = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/properties/1", updateDto);
            var updatedProperty = await response.Content.ReadFromJsonAsync<PropertyDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedProperty.Should().NotBeNull();
            updatedProperty!.Name.Should().Be(updateDto.Name);
            updatedProperty.Price.Should().Be(updateDto.Price);
        }

        [Test]
        public async Task DeleteProperty_ShouldDeleteProperty_WhenPropertyExists()
        {
            // First, create a property to delete
            var newProperty = new CreatePropertyDto
            {
                Name = "Property to Delete",
                Address = "123 Delete Street",
                Price = 100000,
                CodeInternal = "DELETE001",
                Year = 2023,
                IdOwner = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/properties", newProperty);
            var createdProperty = await createResponse.Content.ReadFromJsonAsync<PropertyDto>();

            // Act - Delete the property
            var deleteResponse = await _client.DeleteAsync($"/api/properties/{createdProperty!.IdProperty}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify it was actually deleted
            var getResponse = await _client.GetAsync($"/api/properties/{createdProperty.IdProperty}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Owners Integration Tests

        [Test]
        public async Task GetAllOwners_ShouldReturnAllOwners()
        {
            // Act
            var response = await _client.GetAsync("/api/owners");
            var owners = await response.Content.ReadFromJsonAsync<List<OwnerDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            owners.Should().NotBeNull();
            owners.Should().HaveCountGreaterOrEqualTo(2); // At least the seeded owners
            owners.Should().Contain(o => o.Name == "John Doe");
            owners.Should().Contain(o => o.Name == "Jane Smith");
        }

        [Test]
        public async Task GetOwnerById_ShouldReturnOwner_WhenOwnerExists()
        {
            // Act
            var response = await _client.GetAsync("/api/owners/1");
            var owner = await response.Content.ReadFromJsonAsync<OwnerDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            owner.Should().NotBeNull();
            owner!.IdOwner.Should().Be(1);
            owner.Name.Should().Be("John Doe");
        }

        [Test]
        public async Task CreateOwner_ShouldCreateOwner_WhenValidDataProvided()
        {
            // Arrange
            var newOwner = new CreateOwnerDto
            {
                Name = "New Test Owner",
                Address = "789 New Owner Street",
                Birthday = new DateOnly(1988, 3, 10),
                DocumentNumber = "11111111",
                Email = "newowner@example.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/owners", newOwner);
            var createdOwner = await response.Content.ReadFromJsonAsync<OwnerDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdOwner.Should().NotBeNull();
            createdOwner!.Name.Should().Be(newOwner.Name);
            createdOwner.DocumentNumber.Should().Be(newOwner.DocumentNumber);
        }

        #endregion
    }
}