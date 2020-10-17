using System;
using AutoFixture;
using BecomingPrepper.Api.Controllers.ProgressTracker;
using BecomingPrepper.Core.ProgressTrackerProcessor;
using BecomingPrepper.Data.Enums;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.ProgressTracker
{
    [Trait("Unit", "GetProgressTrack")]
    public class GetShould
    {
        private Mock<IProgressTracker> _mockProgressTracker;
        private Mock<ILogManager> _mockLogManager;
        private ProgressTrackerController _progressTrackerController;
        private Fixture _fixture;
        public GetShould()
        {
            _mockProgressTracker = new Mock<IProgressTracker>();
            _mockLogManager = new Mock<ILogManager>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _progressTrackerController = new ProgressTrackerController(_mockProgressTracker.Object, _mockLogManager.Object);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdProvided()
        {
            _progressTrackerController.GetProgress(string.Empty, _fixture.Create<Objective>(), _fixture.Create<int>()).Should().BeOfType<NotFoundResult>("No accountId was provided");
        }

        [Fact]
        public void ReturnNotFoundWhenInvalidFamilySizeProvided()
        {
            _progressTrackerController.GetProgress(_fixture.Create<string>(), Objective.FiveYear, -1).Should().BeOfType<BadRequestObjectResult>("Invalid Family Size was provided");
        }

        [Fact]
        public void ReturnOkWhenProgressTrackingIsReturned()
        {
            _progressTrackerController.GetProgress(_fixture.Create<string>(), Objective.FiveYear, 3).Should().BeOfType<OkObjectResult>("Progress Tracker was returned");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockProgressTracker.Setup(pt => pt.GetProgress(It.IsAny<string>(), It.IsAny<Objective>(), It.IsAny<int>())).Throws<Exception>();
            _progressTrackerController.GetProgress(_fixture.Create<string>(), _fixture.Create<Objective>(), _fixture.Create<int>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogManager.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
