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
        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act & Assert
            Action act = () => new PropertyRepository(default!);
            
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
