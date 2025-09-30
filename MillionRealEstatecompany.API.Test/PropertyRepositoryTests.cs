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
    public class PropertyRepositoryTests
    {
        private Mock<MongoDbContext> _mockContext;
        private Mock<IMongoCollection<Property>> _mockCollection;
        private PropertyRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<MongoDbContext>();
            _mockCollection = new Mock<IMongoCollection<Property>>();
            _mockContext.Setup(x => x.Properties).Returns(_mockCollection.Object);
            _repository = new PropertyRepository(_mockContext.Object);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act & Assert
            Action act = () => new PropertyRepository(null);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
