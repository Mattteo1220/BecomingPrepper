using System;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.FoodStorageInventoryTests
{
    [Trait("Unit", "DeleteInventory")]
    public class DeleteInventoryShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IRepository<FoodStorageInventoryEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public DeleteInventoryShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageInventoryEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoEntitySupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullEntityTest = () => inventoryUtility.DeleteInventory(null);
            nullEntityTest.Should().Throw<ArgumentNullException>("No Entity was supplied");
        }

        [Fact]
        public void CallDelete()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.DeleteInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockInventoryRepo.Verify(ir => ir.Delete(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.DeleteInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenDeleteThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Delete(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.DeleteInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
