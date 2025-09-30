using AutoMapper;
using FluentAssertions;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;
using MillionRealEstatecompany.API.Services;
using Moq;
using NUnit.Framework;

namespace MillionRealEstatecompany.API.Test
{
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IMapper> _mockMapper;
        private OwnerService _service;

        [SetUp]
        public void Setup()
        {
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new OwnerService(_mockOwnerRepository.Object, _mockPropertyRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllOwnersAsync_ShouldReturnMappedOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner { Name = "John Doe", Email = "john@test.com" },
                new Owner { Name = "Jane Smith", Email = "jane@test.com" }
            };

            var ownerDtos = new List<OwnerDto>
            {
                new OwnerDto { Name = "John Doe", Email = "john@test.com" },
                new OwnerDto { Name = "Jane Smith", Email = "jane@test.com" }
            };

            _mockOwnerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(owners);
            _mockMapper.Setup(x => x.Map<IEnumerable<OwnerDto>>(owners)).Returns(ownerDtos);

            // Act
            var result = await _service.GetAllOwnersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Act & Assert
            Action act = () => new OwnerService(null, _mockPropertyRepository.Object, _mockMapper.Object);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
