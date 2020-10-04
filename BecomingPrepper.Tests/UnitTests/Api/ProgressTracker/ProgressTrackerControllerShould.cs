using System;
using BecomingPrepper.Api.Controllers.ProgressTracker;
using BecomingPrepper.Core.ProgressTrackerProcessor;
using BecomingPrepper.Logger;
using FluentAssertions;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.ProgressTracker
{
    [Trait("Unit", "ProgressTrackerCTOR")]
    public class ProgressTrackerControllerShould
    {
        private Mock<IProgressTracker> _mockProgressTracker;
        private Mock<ILogManager> _mockLogManager;
        private ProgressTrackerController _progressTrackerController;
        public ProgressTrackerControllerShould()
        {
            _mockProgressTracker = new Mock<IProgressTracker>();
            _mockLogManager = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoRecommendServiceProvided()
        {
            Action nullRecommendService = () => new ProgressTrackerController(null, _mockLogManager.Object);
            nullRecommendService.Should().Throw<ArgumentNullException>("No Recommend service was not provided");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoRecommendLoggerProvided()
        {
            Action nullRecommendService = () => new ProgressTrackerController(_mockProgressTracker.Object, null);
            nullRecommendService.Should().Throw<ArgumentNullException>("No LogManager was not provided");
        }


    }
}
