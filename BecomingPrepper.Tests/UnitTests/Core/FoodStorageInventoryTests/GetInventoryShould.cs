using System;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.FoodStorageInventoryTests
{
    [Trait("Unit", "GetInventory")]
    public class GetInventoryShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IGridFSBucket> _mockBucket;
        private Mock<IGallery> _gallery;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public GetInventoryShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockBucket = new Mock<IGridFSBucket>();
            _gallery = new Mock<IGallery>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoEntitySupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _gallery.Object, _mockBucket.Object, _mockLogger.Object);
            Action noAccountIdTest = () => inventoryUtility.GetInventory(" ");
            noAccountIdTest.Should().Throw<ArgumentNullException>("No accountId was supplied");
        }

        [Fact]
        public void CallGet()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _gallery.Object, _mockBucket.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockInventoryRepo.Verify(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _gallery.Object, _mockBucket.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenGetThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _gallery.Object, _mockBucket.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
