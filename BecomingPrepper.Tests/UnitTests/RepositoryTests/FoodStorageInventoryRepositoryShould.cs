using System;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class FoodStorageInventoryRepositoryShould
    {
        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action foodStorageInventoryRepository;

            //Act
            foodStorageInventoryRepository = () => new FoodStorageInventoryRepository(null, "FoodStorageInventory");

            //Assert
            foodStorageInventoryRepository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var foodStorageInventoryRepository = new FoodStorageInventoryRepository(mockDatabase.Object.MongoDatabase, "FoodStorageInventory");
            foodStorageInventoryRepository.Dispose();

            //Asssert
            foodStorageInventoryRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
