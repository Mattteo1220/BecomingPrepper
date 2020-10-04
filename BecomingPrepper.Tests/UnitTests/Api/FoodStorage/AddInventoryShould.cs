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
    [Trait("Unit", "AddInventoryTests")]
    public class AddInventoryShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private Mock<IMapper> _mockMapper;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public AddInventoryShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoArgumentProvided()
        {
            _foodStorageController.AddInventory(null).Should().BeOfType<NotFoundResult>("No InventoryInfo was provided");
        }

        [Fact]
        public void ReturnOkWhenInventoryIsAdded()
        {
            _foodStorageController.AddInventory(_fixture.Create<FoodStorageInventoryInfo>()).Should().BeOfType<OkObjectResult>("The inventory was added");
            _mockMapper.Verify(m => m.Map<FoodStorageEntity>(It.IsAny<FoodStorageInventoryInfo>()), Times.Once);
        }

        [Fact]
        public void ReturnNotFoundWhenAnExceptionIsThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.AddInventory(It.IsAny<FoodStorageEntity>())).Throws<Exception>();
            _foodStorageController.AddInventory(_fixture.Create<FoodStorageInventoryInfo>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
