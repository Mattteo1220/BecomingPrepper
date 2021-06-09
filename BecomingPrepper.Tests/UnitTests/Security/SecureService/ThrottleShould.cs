using System;
using BecomingPrepper.Data.Entities.Endpoint;
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
        private EndpointEntity _endpointEntity;
        public ThrottleShould()
        {
            _endpointEntity = new EndpointEntity();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _serviceCollection = new ServiceCollection();
            _mockResultExecutingContext = new Mock<ResultExecutingContext>();
        }

        [Theory]
        [InlineData(null)]
        public void ThrowArgumentNullExceptionWhenNoKeyProvided(EndpointEntity entity)
        {
            Action nullKey = () => new Throttle(entity);
            nullKey.Should().Throw<ArgumentNullException>("No Key was provided");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void ThrowArgumentOutOfRangeExceptionWhenNoValidRequestLimitIsProvided(int requestLimit)
        {
            _endpointEntity.RequestLimit = requestLimit;
            Action invalidRequestLimit = () => new Throttle(_endpointEntity);
            invalidRequestLimit.Should().Throw<ArgumentOutOfRangeException>("An Invalid Request Limit was provided");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-200)]
        public void ThrowArgumentOutOfRangeExceptionWhenNoValidTimeoutInSecondsIsProvided(int timeoutInSeconds)
        {
            _endpointEntity.Timeout = timeoutInSeconds;
            Action invalidRequestLimit = () => new Throttle(_endpointEntity);
            invalidRequestLimit.Should().Throw<ArgumentOutOfRangeException>("An Invalid timeoutInSeconds was provided");
        }
    }
}
