﻿using System;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.FoodStorageInventoryTests
{
    [Trait("Unit", "AddInventory")]
    public class AddInventoryShould
    {
        private Mock<ILogManager> _mockLogger;
        private Mock<IFileDetailRepository> _mockGalleryRepo;
        private Mock<IChunkRepository> _mockGalleryImageHelperRepo;
        private Mock<IRepository<FoodStorageEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public AddInventoryShould()
        {
            _mockLogger = new Mock<ILogManager>();
            _mockGalleryRepo = new Mock<IFileDetailRepository>();
            _mockGalleryImageHelperRepo = new Mock<IChunkRepository>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageEntity>>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenIRepositorySupplied()
        {
            Action nullParameterTest = () => new InventoryUtility(null, _mockLogger.Object);
            nullParameterTest.Should().Throw<ArgumentNullException>("There was No IRepository supplied");
        }

        [Fact]
        public void ThrowWhenIExceptionLoggerSupplied()
        {
            Action nullParameterTest = () => new InventoryUtility(_mockInventoryRepo.Object, null);
            nullParameterTest.Should().Throw<ArgumentNullException>("There was No ILogManager supplied");
        }

        [Fact]
        public void ThrowWhenNoArgumentSupplied()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action nullParameterTest = () => inventoryUtility.AddInventory(null);
            nullParameterTest.Should().Throw<ArgumentNullException>("There was No Entity supplied");
        }

        [Fact]
        public void CallAddToCreateAnInventory()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventory(_fixture.Create<FoodStorageEntity>());
            _mockInventoryRepo.Verify(ir => ir.Add(It.IsAny<FoodStorageEntity>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventory(_fixture.Create<FoodStorageEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenAddThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Add(It.IsAny<FoodStorageEntity>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            Action errorTest = () => inventoryUtility.AddInventory(_fixture.Create<FoodStorageEntity>());
            errorTest.Should().Throw<Exception>();
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
