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
    [Trait("Unit", "UpdateInventoryItemTests")]
    public class UpdateInventoryItemShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private Mock<IMapper> _mockMapper;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public UpdateInventoryItemShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdIsProvided()
        {
            _foodStorageController.UpdateInventoryItem(string.Empty, _fixture.Create<InventoryInfo>()).Should().BeOfType<NotFoundResult>("No AccountId was provided");
        }

        [Fact]
        public void ReturnNotFoundWhenNoInventoryInfoIsProvided()
        {
            _foodStorageController.UpdateInventoryItem(_fixture.Create<string>(), null).Should().BeOfType<NotFoundResult>("No inventoryInfo was provided");
        }

        [Fact]
        public void ReturnOkWhenInventoryItemIsUpdatedSuccessfully()
        {
            _foodStorageController.UpdateInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryInfo>()).Should().BeOfType<OkObjectResult>("The inventory Item should have been updated.");
            _mockMapper.Verify(m => m.Map<InventoryEntity>(It.IsAny<InventoryInfo>()), Times.Once);
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.UpdateInventoryItem(It.IsAny<string>(), It.IsAny<InventoryEntity>())).Throws<Exception>();
            _foodStorageController.UpdateInventoryItem(_fixture.Create<string>(), _fixture.Create<InventoryInfo>()).Should().BeOfType<NotFoundResult>();
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
