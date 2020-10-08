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
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.FoodStorageInventoryTests
{
    [Trait("Unit", "GetInventoryItem")]
    public class GetInventoryItemShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IGalleryFileHelperRepository> _mockGalleryRepo;
        private Mock<IGalleryImageHelperRepository> _mockGalleryImageHelperRepo;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public GetInventoryItemShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockGalleryRepo = new Mock<IGalleryFileHelperRepository>();
            _mockGalleryImageHelperRepo = new Mock<IGalleryImageHelperRepository>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoItemIdSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            Action noItemIdTest = () => inventoryUtility.GetInventoryItem(_fixture.Create<string>(), null);
            noItemIdTest.Should().Throw<ArgumentNullException>("No ItemId was supplied");
        }

        [Fact]
        public void ThrowWhenNoAccountIdSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            Action noItemIdTest = () => inventoryUtility.GetInventoryItem(" ", _fixture.Create<string>());
            noItemIdTest.Should().Throw<ArgumentNullException>("No accountId was supplied");
        }

        [Fact]
        public void CallGet()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryEntity>();
            inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            _mockInventoryRepo.Verify(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            _mockInventoryRepo.Setup(i => i.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>())).Returns(_fixture.Create<FoodStorageEntity>());
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryEntity>();
            inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public void NotCallLogInformationWhenGetThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.ItemEntity = _fixture.Create<InventoryEntity>();
            Action errorTest = () => inventoryUtility.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>());
            errorTest.Should().Throw<Exception>();
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
