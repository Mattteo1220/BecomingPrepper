using System;
using AutoMapper;
using BecomingPrepper.Api.Controllers.PrepGuide;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.Pilot
{
    [Trait("Unit", "PilotControllerCTOR")]
    public class PilotControllerShould
    {
        private Mock<IPrepGuide> _mockPrepGuide;
        private PilotController _pilotController;
        private Mock<IMapper> _mapper;
        private Mock<ILogManager> _mockLogger;
        public PilotControllerShould()
        {
            _mockPrepGuide = new Mock<IPrepGuide>();
            _mockLogger = new Mock<ILogManager>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoPrepGuideIsProvided()
        {
            Action nullPrepGuideTest = () => new PilotController(null, _mapper.Object, _mockLogger.Object);
            nullPrepGuideTest.Should().Throw<ArgumentNullException>("No PrepGuide was provided");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoLoggerProvided()
        {
            Action nullPrepGuideTest = () => new PilotController(_mockPrepGuide.Object, _mapper.Object, null);
            nullPrepGuideTest.Should().Throw<ArgumentNullException>("No LogManager was provided");
        }
    }
}
