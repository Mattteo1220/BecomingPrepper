using System;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.FoodStorageInventoryTests
{
    public class AddInventoryShould
    {
        private Mock<IExceptionLogger> _mockLogger;
        private Mock<IRepository<FoodStorageInventoryEntity>> _mockInventoryRepo;
        private Fixture _fixture;
        public AddInventoryShould()
        {
            _mockLogger = new Mock<IExceptionLogger>();
            _mockInventoryRepo = new Mock<IRepository<FoodStorageInventoryEntity>>();
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
            nullParameterTest.Should().Throw<ArgumentNullException>("There was No IExceptionLogger supplied");
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
            inventoryUtility.AddInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockInventoryRepo.Verify(ir => ir.Add(It.IsAny<FoodStorageInventoryEntity>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenAddThrowsAnException()
        {
            _mockInventoryRepo.Setup(ir => ir.Add(It.IsAny<FoodStorageInventoryEntity>())).Throws<Exception>();
            var inventoryUtility = new InventoryUtility(_mockInventoryRepo.Object, _mockLogger.Object);
            inventoryUtility.AddInventory(_fixture.Create<FoodStorageInventoryEntity>());
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
