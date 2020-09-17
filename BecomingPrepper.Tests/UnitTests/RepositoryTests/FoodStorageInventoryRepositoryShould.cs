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
        private Mock<IMongoCollection<FoodStorageInventoryEntity>> _mockFoodStorageInventoryCollection;
        private Mock<IExceptionLogger> _mockLogger;
        private Mock<IRepository<FoodStorageInventoryEntity>> _mockInventoryRepository;
        private Fixture _fixture;
        public FoodStorageInventoryRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockFoodStorageInventoryCollection = new Mock<IMongoCollection<FoodStorageInventoryEntity>>();
            _mockLogger = new Mock<IExceptionLogger>();
            _mockInventoryRepository = new Mock<IRepository<FoodStorageInventoryEntity>>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action foodStorageInventoryRepository;

            //Act
            foodStorageInventoryRepository = () => new FoodStorageInventoryRepository(null, _mockLogger.Object);

            //Assert
            foodStorageInventoryRepository.Should().Throw<ArgumentNullException>(because: "No Collection was supplied");
        }

        [Fact]
        public void ThrowIfNoExceptionLoggerIsSupplied()
        {
            //Arrange
            Action foodStorageInventoryRepository;

            //Act
            foodStorageInventoryRepository = () => new FoodStorageInventoryRepository(_mockFoodStorageInventoryCollection.Object, null);

            //Assert
            foodStorageInventoryRepository.Should().Throw<ArgumentNullException>(because: "No ExceptionLogger was supplied");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var foodStorageInventoryRepository = new FoodStorageInventoryRepository(_mockFoodStorageInventoryCollection.Object, _mockLogger.Object);
            foodStorageInventoryRepository.Dispose();

            //Asssert
            foodStorageInventoryRepository.Collection.Should().BeNull(because: "It was disposed of");
        }
    }
}
