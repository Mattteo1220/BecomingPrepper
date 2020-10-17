using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.PrepGuide;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.Pilot
{
    [Trait("Unit", "DeleteTip")]
    public class DeleteTipShould
    {
        private Mock<IPrepGuide> _mockPrepGuide;
        private PilotController _pilotController;
        private Mock<IMapper> _mapper;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public DeleteTipShould()
        {
            _mockPrepGuide = new Mock<IPrepGuide>();
            _mockLogger = new Mock<ILogManager>();
            _mapper = new Mock<IMapper>();
            _pilotController = new PilotController(_mockPrepGuide.Object, _mapper.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoTipIdProvided()
        {
            _pilotController.DeleteTip(string.Empty).Should().BeOfType<NotFoundResult>("No TipId was provided");
        }

        [Fact]
        public void ReturnOkWhenTipIsDeletedSuccessfully()
        {
            _pilotController.DeleteTip(_fixture.Create<string>()).Should().BeOfType<OkObjectResult>("The tip was deleted Successfully");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockPrepGuide.Setup(pg => pg.Delete(It.IsAny<ObjectId>(), It.IsAny<string>(), false)).Throws<Exception>();
            _pilotController.DeleteTip(_fixture.Create<string>()).Should().BeOfType<NotFoundResult>("An Exception was Thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
