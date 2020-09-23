using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class PrepGuideRepositoryShould
    {
        private Mock<IMongoCollection<PrepGuideEntity>> _mockFoodStorageInventoryCollection;
        private Mock<IExceptionLogger> _mockLogger;
        private Fixture _fixture;
        public PrepGuideRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockFoodStorageInventoryCollection = new Mock<IMongoCollection<PrepGuideEntity>>();
            _mockLogger = new Mock<IExceptionLogger>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseSupplied()
        {
            //Arrange
            Action PrepGuideRepository;

            //Act
            PrepGuideRepository = () => new PrepGuideRepository(null, _mockLogger.Object);

            //Assert
            PrepGuideRepository.Should().Throw<ArgumentNullException>("No Collection was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var prepGuideRepository = new PrepGuideRepository(_mockFoodStorageInventoryCollection.Object, _mockLogger.Object);
            prepGuideRepository.Dispose();

            //Asssert
            prepGuideRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
