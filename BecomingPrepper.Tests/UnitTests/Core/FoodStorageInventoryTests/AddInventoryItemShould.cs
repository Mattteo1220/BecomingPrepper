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
    [Trait("Unit", "AddInventoryItem")]
    public class AddInventoryItemShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IFileDetailRepository> _mockGalleryRepo;
        private Mock<IChunkRepository> _mockGalleryImageHelperRepo;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public AddInventoryItemShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _mockGalleryImageHelperRepo = new Mock<IChunkRepository>();
            _mockGalleryRepo = new Mock<IFileDetailRepository>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void ThrowArgumentNullExceptionWhenNoAccountIdSupplied(string accountId)
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullAccountIdTest = () => inventoryUtility.AddInventoryItem(accountId, new InventoryEntity());
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
            inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            _mockInventoryRepo.Verify(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageEntity>>(), It.IsAny<UpdateDefinition<FoodStorageEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenAddThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Update(It.IsAny<FilterDefinition<FoodStorageEntity>>(), It.IsAny<UpdateDefinition<FoodStorageEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action errorTest = () => inventoryUtility.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryEntity>());
            errorTest.Should().Throw<Exception>();
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
