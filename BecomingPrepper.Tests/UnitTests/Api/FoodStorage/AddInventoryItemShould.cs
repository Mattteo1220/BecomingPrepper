using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.Inventory;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.FoodStorage
{
    [Trait("Unit", "AddInventoryItem")]
    public class AddInventoryItemShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private Mock<IMapper> _mockMapper;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public AddInventoryItemShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdIsSupplied()
        {
            _foodStorageController.AddInventoryItem(string.Empty, _fixture.Create<InventoryInfo>()).Should().BeOfType<NotFoundResult>("No Account Id was provided");
        }

        [Fact]
        public void ReturnNotFoundWhenNoInventoryInfoIsSupplied()
        {
            _foodStorageController.AddInventoryItem(_fixture.Create<string>(), null).Should().BeOfType<NotFoundResult>("No inventoryItem Info was provided");
        }

        [Fact]
        public void ReturnOkWhenItemIsAdded()
        {
            _foodStorageController.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryInfo>()).Should().BeOfType<OkObjectResult>("The item was added");
            _mockMapper.Verify(m => m.Map<InventoryEntity>(It.IsAny<InventoryInfo>()), Times.Once);
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.AddInventoryItem(It.IsAny<string>(), It.IsAny<InventoryEntity>())).Throws<Exception>();
            _foodStorageController.AddInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryInfo>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
