using System;
using AutoFixture;
using BecomingPrepper.Core.RecommenedQuantitiesUtility;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RecommendServiceTests
{
    public class AddRecommendedAmountShould
    {
        private IRecommendService _recommendService;
        private Mock<IRepository<RecommendedQuantityAmountEntity>> _mockRecommendRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Fixture _fixture;

        public AddRecommendedAmountShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockExceptionLogger = new Mock<ILogManager>();
            _mockRecommendRepo = new Mock<IRepository<RecommendedQuantityAmountEntity>>();
            _recommendService = new RecommendService(_mockRecommendRepo.Object, _mockExceptionLogger.Object);
        }

        [Fact]
        public void Throw_WhenNoArgumentsProvided()
        {
            Action nullArgumentTest = () => _recommendService.AddRecommendedAmount(null);
            nullArgumentTest.Should().Throw<ArgumentNullException>("No new recommended Entity was provided");
        }

        [Fact]
        public void CallAddInRecommendedRepo()
        {
            var recommendedQuantityAmountEntity = _fixture.Create<RecommendedQuantityAmountEntity>();
            _mockRecommendRepo.Setup(rr => rr.Add(It.IsAny<RecommendedQuantityAmountEntity>()));
            _recommendService.AddRecommendedAmount(recommendedQuantityAmountEntity);
            _mockRecommendRepo.Verify(rr => rr.Add(It.IsAny<RecommendedQuantityAmountEntity>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation()
        {
            var recommendedQuantityAmountEntity = _fixture.Create<RecommendedQuantityAmountEntity>();
            _mockRecommendRepo.Setup(rr => rr.Add(It.IsAny<RecommendedQuantityAmountEntity>()));
            _recommendService.AddRecommendedAmount(recommendedQuantityAmountEntity);
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformationWhenAddThrows()
        {
            var recommendedQuantityAmountEntity = _fixture.Create<RecommendedQuantityAmountEntity>();
            _mockRecommendRepo.Setup(rr => rr.Add(It.IsAny<RecommendedQuantityAmountEntity>())).Throws<Exception>();
            Action errorTest = () => _recommendService.AddRecommendedAmount(recommendedQuantityAmountEntity);
            errorTest.Should().Throw<Exception>();
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
