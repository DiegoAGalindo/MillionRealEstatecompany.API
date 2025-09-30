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
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private Mock<IMapper> _mockMapper;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new PropertyService(_mockPropertyRepository.Object, _mockOwnerRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllPropertiesAsync_ShouldReturnMappedProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Name = "Property 1", Address = "Address 1" },
                new Property { Name = "Property 2", Address = "Address 2" }
            };

            var propertyDtos = new List<PropertyDto>
            {
                new PropertyDto { Name = "Property 1", Address = "Address 1" },
                new PropertyDto { Name = "Property 2", Address = "Address 2" }
            };

            _mockPropertyRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(properties);
            _mockMapper.Setup(x => x.Map<IEnumerable<PropertyDto>>(properties)).Returns(propertyDtos);

            // Act
            var result = await _service.GetAllPropertiesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Act & Assert
            Action act = () => new PropertyService(null, _mockOwnerRepository.Object, _mockMapper.Object);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
