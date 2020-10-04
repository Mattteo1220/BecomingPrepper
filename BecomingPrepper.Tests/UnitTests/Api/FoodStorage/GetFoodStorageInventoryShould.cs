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
    public class GetFoodStorageInventoryShould
    {
        private readonly Mock<IInventoryUtility> _mockInventoryUtility;
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly FoodStorageController _foodStorageController;
        private readonly Fixture _fixture;
        public GetFoodStorageInventoryShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            var mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
            _foodStorageController = new FoodStorageController(_mockInventoryUtility.Object, mockMapper.Object, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdSupplied()
        {
            _foodStorageController.GetFoodStorageInventory(string.Empty).Should().BeOfType<NotFoundResult>("No AccountId was provided");
        }

        [Fact]
        public void ReturnNoContentWhenNothingReturned()
        {
            _foodStorageController.GetFoodStorageInventory(_fixture.Create<string>()).Should().BeOfType<NoContentResult>("an Invalid AccountId was provided");
        }

        [Fact]
        public void ReturnOkWithInventory()
        {
            _mockInventoryUtility.Setup(iu => iu.GetInventory(It.IsAny<string>())).Returns(_fixture.Create<FoodStorageEntity>());
            var response = _foodStorageController.GetFoodStorageInventory(_fixture.Create<string>()) as OkObjectResult;
            response.Should().BeOfType<OkObjectResult>();
            response?.Value.Should().NotBeNull("An inventory should have been returned");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockInventoryUtility.Setup(iu => iu.GetInventory(It.IsAny<string>())).Throws<Exception>();
            _foodStorageController.GetFoodStorageInventory(_fixture.Create<string>()).Should().BeOfType<NotFoundResult>();
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
