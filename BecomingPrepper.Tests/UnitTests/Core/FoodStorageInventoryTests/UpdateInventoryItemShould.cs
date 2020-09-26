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
    public class UpdateInventoryItemShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public UpdateInventoryItemShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoEntitySupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullEntityTest = () => inventoryUtility.UpdateInventoryItem(_fixture.Create<string>(), null);
            nullEntityTest.Should().Throw<ArgumentNullException>("No Entity was supplied");
        }

        [Fact]
        public void ThrowWhenNoAccountIdSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullEntityTest = () => inventoryUtility.UpdateInventoryItem(" ", _fixture.Create<InventoryEntity>());
            nullEntityTest.Should().Throw<ArgumentNullException>("No AccountId was supplied");
        }

        [Fact]
        public void CallUpdate()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.UpdateInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            _mockInventoryRepo.Verify(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageEntity>>(), It.IsAny<UpdateDefinition<FoodStorageEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.UpdateInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenUpdateThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageEntity>>(), It.IsAny<UpdateDefinition<FoodStorageEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.UpdateInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
