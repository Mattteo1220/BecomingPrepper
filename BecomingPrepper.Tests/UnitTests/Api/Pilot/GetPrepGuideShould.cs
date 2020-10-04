using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.PrepGuide;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.Pilot
{
    [Trait("Unit", "GetPrepGuide")]
    public class GetPrepGuideShould
    {
        private Mock<IPrepGuide> _mockPrepGuide;
        private PilotController _pilotController;
        private Mock<IMapper> _mapper;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public GetPrepGuideShould()
        {
            _mockPrepGuide = new Mock<IPrepGuide>();
            _mockLogger = new Mock<ILogManager>();
            _mapper = new Mock<IMapper>();
            _pilotController = new PilotController(_mockPrepGuide.Object, _mapper.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNoContentWhenNoPrepGuideFound()
        {
            var response = _pilotController.GetPrepGuide();
            response.Should().BeOfType<NoContentResult>("The Prep guide should not be returned");
        }

        [Fact]
        public void ReturnOkWithPrepGuide()
        {
            _mockPrepGuide.Setup(pg => pg.GetPrepGuide()).Returns(_fixture.Create<PrepGuideEntity>());
            var response = _pilotController.GetPrepGuide() as OkObjectResult;
            response.Should().BeOfType<OkObjectResult>("The Prep guide should not be returned");
            response?.Value.Should().NotBeNull("The Prep Guide should have been returned");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockPrepGuide.Setup(pg => pg.GetPrepGuide()).Throws<Exception>();
            _pilotController.GetPrepGuide().Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
