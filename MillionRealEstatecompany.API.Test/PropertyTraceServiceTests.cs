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
    public class PropertyTraceServiceTests
    {
        private Mock<IPropertyTraceRepository> _mockPropertyTraceRepository;
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IMapper> _mockMapper;
        private PropertyTraceService _service;

        [SetUp]
        public void Setup()
        {
            _mockPropertyTraceRepository = new Mock<IPropertyTraceRepository>();
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new PropertyTraceService(_mockPropertyTraceRepository.Object, _mockPropertyRepository.Object, _mockMapper.Object);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Act & Assert
            Action act = () => new PropertyTraceService(null, _mockPropertyRepository.Object, _mockMapper.Object);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
