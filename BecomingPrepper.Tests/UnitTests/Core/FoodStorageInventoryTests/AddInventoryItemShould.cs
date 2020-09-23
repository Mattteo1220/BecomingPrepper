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
    public class AddInventoryItemShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IRepository<FoodStorageInventoryEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public AddInventoryItemShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageInventoryEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void ThrowArgumentNullExceptionWhenNoAccountIdSupplied(string accountId)
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullAccountIdTest = () => inventoryUtility.AddInventoryItem(accountId, new InventoryItemEntity());
            nullAccountIdTest.Should().Throw<ArgumentNullException>("No AccountId was supplied");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoEntitySupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullAccountIdTest = () => inventoryUtility.AddInventoryItem(_fixture.Create<string>(), null);
            nullAccountIdTest.Should().Throw<ArgumentNullException>("No InventoryItem Entity was supplied");
        }

        [Fact]
        public void CallAddInventoryItem()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryItemEntity>());
            _mockInventoryRepo.Verify(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>(), It.IsAny<UpdateDefinition<FoodStorageInventoryEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryItemEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenAddThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>(), It.IsAny<UpdateDefinition<FoodStorageInventoryEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryItemEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
