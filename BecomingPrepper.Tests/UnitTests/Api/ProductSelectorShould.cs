using System;
using System.Collections.Generic;
using BecomingPrepper.Api.Controllers;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api
{
    [Trait("Unit", "ProductSelectorAPI")]
    public class ProductSelectorShould
    {
        private Mock<ILogManager> _mockLogManager;
        private ProductSelectorController _productSelector;
        private Mock<IMemoryCache> _mockMemoryCache;
        public ProductSelectorShould()
        {
            _mockLogManager = new Mock<ILogManager>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _productSelector = new ProductSelectorController(_mockLogManager.Object);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoLogManagerProvided()
        {
            Action nullConstructorArgumentTest = () => new ProductSelectorController(null);
            nullConstructorArgumentTest.Should().Throw<ArgumentNullException>("No log Manager was provided");
        }

        [Theory]
        [InlineData(1, "All Purpose Flour")]
        [InlineData(2, "SeaFood")]
        [InlineData(3, "Palm Oil")]
        [InlineData(4, "Chickpeas Garbanzo")]
        [InlineData(5, "Evaporated Milk")]
        [InlineData(6, "Corn Syrup")]
        [InlineData(7, "Balsamic Vinegar")]
        [InlineData(8, "Chocolate Cherries")]
        [InlineData(9, "Pinto Beans")]
        [InlineData(10, "Fifty Five Gallon Water Drum")]
        public void ReturnCorrectType(int category, string expectedString)
        {
            var response = _productSelector.GetProducts(category) as OkObjectResult;
            response.Should().BeOfType<OkObjectResult>();
            var value = (List<string>)response?.Value;
            value.Should().Contain(expectedString);
        }

        [Fact]
        public void ThrowBadRequestWhenInvalidCategoryProvided()
        {
            _productSelector.GetProducts(11).Should().BeOfType<BadRequestObjectResult>("an Invalid Category was provided");
        }
    }
}
