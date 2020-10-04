using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.Inventory;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.FoodStorage
{
    [Trait("Unit", "DeleteInventoryTests")]
    public class DeleteInventoryShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private Mock<IMapper> _mockMapper;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public DeleteInventoryShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdSupplied()
        {
            _foodStorageController.DeleteInventory(string.Empty).Should().BeOfType<NotFoundResult>("No accountId was Provided");
        }

        [Fact]
        public void ReturnOkWhenInventoryDeleted()
        {
            _foodStorageController.DeleteInventory(_fixture.Create<string>()).Should().BeOfType<OkObjectResult>("Inventory was deleted.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.DeleteInventory(It.IsAny<string>())).Throws<Exception>();
            _foodStorageController.DeleteInventory(_fixture.Create<string>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
