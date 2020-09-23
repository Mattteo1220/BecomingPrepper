using System;
using AutoFixture;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class RecommendedQuantityRepositoryShould
    {
        private Mock<IMongoCollection<RecommendedQuantityAmountEntity>> _mockFoodStorageInventoryCollection;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public RecommendedQuantityRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockFoodStorageInventoryCollection = new Mock<IMongoCollection<RecommendedQuantityAmountEntity>>();
            _mockLogger = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action recommendedQuantityRespository;

            //Act
            recommendedQuantityRespository = () => new RecommendedQuantityRepository(null, _mockLogger.Object);

            //Assert
            recommendedQuantityRespository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var recommendedQuantityRepository = new RecommendedQuantityRepository(_mockFoodStorageInventoryCollection.Object, _mockLogger.Object);
            recommendedQuantityRepository.Dispose();

            //Assert
            recommendedQuantityRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
