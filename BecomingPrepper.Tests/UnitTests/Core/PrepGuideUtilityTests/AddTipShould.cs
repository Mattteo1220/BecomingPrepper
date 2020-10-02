using System;
using AutoFixture;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.PrepGuideUtilityTests
{
    public class AddTipShould
    {
        private IPrepGuide _prepGuideUtility;
        private Mock<IRepository<PrepGuideEntity>> _mockPrepGuideRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Fixture _fixture;
        public AddTipShould()
        {
            _mockPrepGuideRepo = new Mock<IRepository<PrepGuideEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _prepGuideUtility = new PrepGuide(_mockPrepGuideRepo.Object, _mockExceptionLogger.Object);
        }

        [Fact]
        public void ThrowArgumentNullException_WhenNoValidObjectIdSupplied()
        {
            Action invalidArgumentsTest = () => _prepGuideUtility.Add(ObjectId.Empty, _fixture.Create<TipEntity>(), true);
            invalidArgumentsTest.Should().Throw<ArgumentNullException>("Invalid Parameters Supplied");
        }

        [Fact]
        public void ThrowArgumentNullException_WhenNoValidTipSupplied()
        {
            Action invalidArgumentsTest = () => _prepGuideUtility.Add(ObjectId.GenerateNewId(), null, true);
            invalidArgumentsTest.Should().Throw<ArgumentNullException>("Invalid Parameters Supplied");
        }

        [Fact]
        public void CallDeleteTip()
        {
            _prepGuideUtility.Add(_fixture.Create<ObjectId>(), _fixture.Create<TipEntity>(), true);
            _mockPrepGuideRepo.Verify(pge => pge.Update(It.IsAny<FilterDefinition<PrepGuideEntity>>(), It.IsAny<UpdateDefinition<PrepGuideEntity>>()), Times.Once());
        }

        [Fact]
        public void CallLogInformation()
        {
            _prepGuideUtility.Add(_fixture.Create<ObjectId>(), _fixture.Create<TipEntity>(), true);
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenExceptionThrownOnDelete()
        {
            _mockPrepGuideRepo.Setup(pgr => pgr.Update(It.IsAny<FilterDefinition<PrepGuideEntity>>(), It.IsAny<UpdateDefinition<PrepGuideEntity>>())).Throws<Exception>();
            Action errorTest = () => _prepGuideUtility.Add(_fixture.Create<ObjectId>(), _fixture.Create<TipEntity>(), true);
            errorTest.Should().Throw<Exception>();
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
