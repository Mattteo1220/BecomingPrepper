using System;
using BecomingPrepper.Security;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.SecureService
{
    [Trait("Unit", "ThrottleTests")]
    public class ThrottleShould
    {
        private readonly Throttle _throttle;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly IServiceCollection _serviceCollection;
        private readonly Mock<ResultExecutingContext> _mockResultExecutingContext;
        public ThrottleShould()
        {
            _throttle = new Throttle("GetProducts", 100, 60);
            _mockMemoryCache = new Mock<IMemoryCache>();
            _serviceCollection = new ServiceCollection();
            _mockResultExecutingContext = new Mock<ResultExecutingContext>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void ThrowArgumentNullExceptionWhenNoKeyProvided(string key)
        {
            Action nullKey = () => new Throttle(key, 1, 1);
            nullKey.Should().Throw<ArgumentNullException>("No Key was provided");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void ThrowArgumentOutOfRangeExceptionWhenNoValidRequestLimitIsProvided(int requestLimit)
        {
            Action invalidRequestLimit = () => new Throttle("GetProducts", requestLimit, 2);
            invalidRequestLimit.Should().Throw<ArgumentOutOfRangeException>("An Invalid Request Limit was provided");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-200)]
        public void ThrowArgumentOutOfRangeExceptionWhenNoValidTimeoutInSecondsIsProvided(int timeoutInSeconds)
        {
            Action invalidRequestLimit = () => new Throttle("GetProducts", 2, timeoutInSeconds);
            invalidRequestLimit.Should().Throw<ArgumentOutOfRangeException>("An Invalid timeoutInSeconds was provided");
        }

        //[Theory]
        //[InlineData(1, false)]
        //[InlineData(2, false)]
        //[InlineData(3, false)]
        //[InlineData(98, false)]
        //[InlineData(99, false)]
        //[InlineData(100, false)]
        //[InlineData(101, true)]
        //[InlineData(102, true)]
        //public void ReturnCorrectlyBasedOnHowManyRequestsHaveBeenSubmitted(int numOfRequests, bool result)
        //{
        //    _serviceCollection.AddMemoryCache();
        //    _mockMemoryCache.Setup(mc => mc.TryGetValue(It.IsAny<string>(), out It.IsAny<Throttle.ThrottleInfo>())).Returns()
        //    _throttle.MemoryCache = _mockMemoryCache.Object;
        //}
    }
}
