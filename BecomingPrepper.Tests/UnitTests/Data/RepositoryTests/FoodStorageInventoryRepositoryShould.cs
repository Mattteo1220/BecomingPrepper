using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class FoodStorageInventoryRepositoryShould
    {
        private Mock<IMongoDatabase> _mockFoodStorageInventoryCollection;
        private Mock<ILogManager> _mockLogger;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepository;
        private Fixture _fixture;
        public FoodStorageInventoryRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockFoodStorageInventoryCollection = new Mock<IMongoDatabase>();
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepository = new Mock<IRepository<FoodStorageEntity>>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action foodStorageInventoryRepository;

            //Act
            foodStorageInventoryRepository = () => new FoodStorageInventoryRepository(null, _mockLogger.Object);

            //Assert
            foodStorageInventoryRepository.Should().Throw<ArgumentNullException>(because: "No _collection was supplied");
        }

        [Fact]
        public void ThrowIfNoExceptionLoggerIsSupplied()
        {
            //Arrange
            Action foodStorageInventoryRepository;

            //Act
            foodStorageInventoryRepository = () => new FoodStorageInventoryRepository(_mockFoodStorageInventoryCollection.Object, null);

            //Assert
            foodStorageInventoryRepository.Should().Throw<ArgumentNullException>(because: "No LogManager was supplied");
        }
    }
}
