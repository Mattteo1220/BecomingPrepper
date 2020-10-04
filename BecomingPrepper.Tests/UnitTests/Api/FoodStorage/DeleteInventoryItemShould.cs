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
    [Trait("Unit", "DeleteInventoryItemTests")]
    public class DeleteInventoryItemShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private Mock<IMapper> _mockMapper;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public DeleteInventoryItemShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdProvided()
        {
            _foodStorageController.DeleteInventoryItem(string.Empty, _fixture.Create<string>()).Should().BeOfType<NotFoundResult>("No AccountId was provided");
        }

        [Fact]
        public void ReturnNotFoundWhenNoItemIdProvided()
        {
            _foodStorageController.DeleteInventoryItem(_fixture.Create<string>(), string.Empty).Should().BeOfType<NotFoundResult>("No ItemId was provided");
        }

        [Fact]
        public void ReturnOkWhenItemIsDeleted()
        {
            _foodStorageController.DeleteInventoryItem(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeOfType<OkObjectResult>("the item was deleted");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.DeleteInventoryItem(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _foodStorageController.DeleteInventoryItem(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
