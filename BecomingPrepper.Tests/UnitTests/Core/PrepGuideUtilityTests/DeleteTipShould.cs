using System;
using AutoFixture;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.PrepGuideUtilityTests
{
    public class DeleteTipShould
    {
        private IPrepGuide _prepGuideUtility;
        private Mock<IRepository<PrepGuideEntity>> _mockPrepGuideRepo;
        private Mock<IExceptionLogger> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public DeleteTipShould()
        {
            _mockPrepGuideRepo = new Mock<IRepository<PrepGuideEntity>>();
            _mockExceptionLogger = new Mock<IExceptionLogger>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _prepGuideUtility = new PrepGuideUtility(_mockPrepGuideRepo.Object, _mockExceptionLogger.Object);
        }

        [Fact]
        public void ThrowArgumentNullException_WhenNoValidObjectIdSupplied()
        {
            Action invalidArgumentsTest = () => _prepGuideUtility.Delete(ObjectId.Empty, _fixture.Create<string>(), true);
            invalidArgumentsTest.Should().Throw<ArgumentNullException>("Invalid Parameters Supplied");
        }

        [Fact]
        public void ThrowArgumentNullException_WhenNoValidTipSupplied()
        {
            Action invalidArgumentsTest = () => _prepGuideUtility.Delete(ObjectId.GenerateNewId(), string.Empty, true);
            invalidArgumentsTest.Should().Throw<ArgumentNullException>("Invalid Parameters Supplied");
        }

        [Fact]
        public void CallDeleteTip()
        {
            _prepGuideUtility.Delete(_fixture.Create<ObjectId>(), _fixture.Create<string>(), true);
            _mockPrepGuideRepo.Verify(pge => pge.Update(It.IsAny<FilterDefinition<PrepGuideEntity>>(), It.IsAny<UpdateDefinition<PrepGuideEntity>>()), Times.Once());
        }

        [Fact]
        public void CallLogInformation()
        {
            _prepGuideUtility.Delete(_fixture.Create<ObjectId>(), _fixture.Create<string>(), true);
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenExceptionThrownOnDelete()
        {
            _mockPrepGuideRepo.Setup(pgr => pgr.Update(It.IsAny<FilterDefinition<PrepGuideEntity>>(), It.IsAny<UpdateDefinition<PrepGuideEntity>>())).Throws<Exception>();
            _prepGuideUtility.Delete(_fixture.Create<ObjectId>(), _fixture.Create<string>(), true);
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
