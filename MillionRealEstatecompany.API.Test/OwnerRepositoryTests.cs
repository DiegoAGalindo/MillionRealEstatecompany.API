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
    public class OwnerRepositoryTests
    {
        private Mock<MongoDbContext> _mockContext;
        private Mock<IMongoCollection<Owner>> _mockCollection;
        private OwnerRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<MongoDbContext>();
            _mockCollection = new Mock<IMongoCollection<Owner>>();
            _mockContext.Setup(x => x.Owners).Returns(_mockCollection.Object);
            _repository = new OwnerRepository(_mockContext.Object);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act & Assert
            Action act = () => new OwnerRepository(null);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
