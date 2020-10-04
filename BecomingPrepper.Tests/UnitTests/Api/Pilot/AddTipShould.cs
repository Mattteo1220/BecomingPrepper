using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.PrepGuide;
using BecomingPrepper.Api.Objects;
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
    [Trait("Unit", "AddTip")]
    public class AddTipShould
    {
        private Mock<IPrepGuide> _mockPrepGuide;
        private PilotController _pilotController;
        private Mock<IMapper> _mapper;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public AddTipShould()
        {
            _mockPrepGuide = new Mock<IPrepGuide>();
            _mockLogger = new Mock<ILogManager>();
            _mapper = new Mock<IMapper>();
            _pilotController = new PilotController(_mockPrepGuide.Object, _mapper.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoTipProvided()
        {
            _pilotController.Post(null).Should().BeOfType<NotFoundResult>("No TipInfo was provided");
        }

        [Fact]
        public void ReturnOkWhenTipIsAddedSuccessfully()
        {
            _pilotController.Post(_fixture.Create<TipInfo>()).Should().BeOfType<OkObjectResult>("Tip was Added successfully");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockPrepGuide.Setup(pg => pg.Add(ObjectId.Empty, It.IsAny<TipEntity>(), false)).Throws<Exception>();
            _pilotController.Post(_fixture.Create<TipInfo>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
