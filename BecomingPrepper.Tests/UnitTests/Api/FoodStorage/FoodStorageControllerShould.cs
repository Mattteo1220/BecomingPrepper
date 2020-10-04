using System;
using AutoMapper;
using BecomingPrepper.Api.Controllers.Inventory;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.FoodStorage
{
    [Trait("Unit", "FoodStorageCTOR")]
    public class FoodStorageControllerShould
    {
        private Mock<IInventoryUtility> _mockInventoryUtility;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogManager> _mockLogManager;
        public FoodStorageControllerShould()
        {
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockMapper = new Mock<IMapper>();
            _mockLogManager = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoInventoryUtilityProvided()
        {
            Action nullParameterTest = () => new FoodStorageController(null, _mockMapper.Object, _mockLogManager.Object);
            nullParameterTest.Should().Throw<ArgumentNullException>("No inventoryUtility was supplied");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoMapperProvided()
        {
            Action nullParameterTest = () => new FoodStorageController(_mockInventoryUtility.Object, null, _mockLogManager.Object);
            nullParameterTest.Should().Throw<ArgumentNullException>("No Mapper was supplied");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoLogManagerProvided()
        {
            Action nullParameterTest = () => new FoodStorageController(_mockInventoryUtility.Object, _mockMapper.Object, null);
            nullParameterTest.Should().Throw<ArgumentNullException>("No LogManager was supplied");
        }
    }
}
