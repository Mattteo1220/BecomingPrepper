using System;
using System.Collections.Generic;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
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
    [Trait("Unit", "GetInventory")]
    public class GetInventoryShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IGalleryImageHelperRepository> _mockGalleryImageHelperRepo;
        private Mock<IGalleryFileHelperRepository> _mockGalleryFileHelperRepo;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public GetInventoryShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockGalleryImageHelperRepo = new Mock<IGalleryImageHelperRepository>();
            _mockGalleryFileHelperRepo = new Mock<IGalleryFileHelperRepository>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoEntitySupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryFileHelperRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            Action noAccountIdTest = () => inventoryUtility.GetInventory(" ");
            noAccountIdTest.Should().Throw<ArgumentNullException>("No accountId was supplied");
        }

        [Fact]
        public void CallGet()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryFileHelperRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockInventoryRepo.Verify(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            _mockInventoryRepo.Setup(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>())).Returns(_fixture.Create<FoodStorageEntity>());
            _mockGalleryFileHelperRepo.Setup(f => f.GetFiles()).Returns(It.IsAny<List<GalleryFileInfoEntity>>());
            _mockGalleryImageHelperRepo.Setup(i => i.GetImage(It.IsAny<ObjectId>())).Returns(It.IsAny<byte[]>());
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryFileHelperRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenGetThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Get(It.IsAny<FilterDefinition<FoodStorageEntity>>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockGalleryFileHelperRepo.Object, _mockGalleryImageHelperRepo.Object, _mockLogger.Object);
            inventoryUtility.GetInventory(_fixture.Create<string>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
