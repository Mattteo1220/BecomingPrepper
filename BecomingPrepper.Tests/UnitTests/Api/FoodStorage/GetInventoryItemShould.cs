using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.Inventory;
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
    [Trait("Unit", "GetInventoryItemTest")]
    public class GetInventoryItemShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public GetInventoryItemShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            var mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Theory]
        [InlineData(" ", "lskdjf")]
        [InlineData("lskdjf", null)]
        public void ReturnNotFoundWhenNoParametersSupplied(string accountId, string itemId)
        {
            _foodStorageController.GetInventoryItem(accountId, itemId).Should().BeOfType<NotFoundResult>("No Valid parameters were supplied");
        }

        [Fact]
        public void ReturnNoContentWhenNothingReturned()
        {
            _foodStorageController.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeOfType<NoContentResult>("No item was returned");
        }

        [Fact]
        public void ReturnOkWithItem()
        {
            _mockInventoryUtility.Setup(iu => iu.GetInventoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(_fixture.Create<InventoryEntity>());
            var response = _foodStorageController.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>()) as OkObjectResult;
            response.Should().BeOfType<OkObjectResult>();
            response?.Value.Should().NotBeNull("An inventory item was returned");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.GetInventoryItem(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _foodStorageController.GetInventoryItem(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeOfType<NotFoundResult>("An exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);

        }
    }
}
