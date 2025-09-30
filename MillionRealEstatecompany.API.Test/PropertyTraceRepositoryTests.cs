using AutoMapper;
using FluentAssertions;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace MillionRealEstatecompany.API.Test
{
    [TestFixture]
    public class PropertyTraceRepositoryTests
    {
        private Mock<MongoDbContext> _mockContext;
        private Mock<IMongoCollection<PropertyTrace>> _mockCollection;
        private PropertyTraceRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<MongoDbContext>();
            _mockCollection = new Mock<IMongoCollection<PropertyTrace>>();
            _mockContext.Setup(x => x.PropertyTraces).Returns(_mockCollection.Object);
            _repository = new PropertyTraceRepository(_mockContext.Object);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act & Assert
            Action act = () => new PropertyTraceRepository(null);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
