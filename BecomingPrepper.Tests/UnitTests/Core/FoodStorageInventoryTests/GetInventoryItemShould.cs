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
    [Trait("Unit", "GetInventoryItem")]
    public class GetInventoryItemShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IRepository<FoodStorageInventoryEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public GetInventoryItemShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageInventoryEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoItemIdSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action noItemIdTest = () => inventoryUtility.GetInventoryItem(_fixture.Create<string>(), null);
            noItemIdTest.Should().Throw<ArgumentNullException>("No ItemId was supplied");
        }

        [Fact]
        public void ThrowWhenNoAccountIdSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action noItemIdTest = () => inventoryUtility.GetInventoryItem(" ", _fixture.Create<string>());
            noItemIdTest.Should().Throw<ArgumentNullException>("No accountId was supplied");
        }

        [Fact]
        public void CallGet()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryItemEntity>();
            inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            _mockInventoryRepo.Verify(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryItemEntity>();
            inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public void NotCallLogInformationWhenGetThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageInventoryEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryItemEntity>();
            inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }
    }
}
