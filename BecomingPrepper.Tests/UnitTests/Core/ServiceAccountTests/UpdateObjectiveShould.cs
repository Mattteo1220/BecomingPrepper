using System;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.ServiceAccountTests
{
    [Trait("Unit", "UpdateObjectiveTests")]
    public class UpdateObjectiveShould
    {
        private IServiceAccount _serviceAccount;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<IExceptionLogger> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public UpdateObjectiveShould()
        {
            _serviceAccount = Mock.Of<IServiceAccount>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<IExceptionLogger>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowWhenNoValidAccountIdSupplied(string accountId)
        {
            Action invalidAccountIdTest = () => _serviceAccount.UpdateObjective(accountId, 1);
            invalidAccountIdTest.Should().Throw<ArgumentNullException>("No Valid AccountId was supplied");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(11)]
        public void ThrowWhenNoValidObjectiveSupplied(int objective)
        {
            Action invalidObjectiveTest = () => _serviceAccount.UpdateObjective(_fixture.Create<string>(), objective);
            invalidObjectiveTest.Should().Throw<InvalidOperationException>("Objective must be a supported type");
        }

        [Fact]
        public void CallUpdateInUserRepository()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateObjective(_fixture.Create<string>(), 5);
            _mockUserRepo.Verify(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasUpdatedTheirEmail()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateObjective(_fixture.Create<string>(), 3);
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformation_WhenUpdateHasThrownAnException()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();

            _serviceAccount.UpdateObjective(_fixture.Create<string>(), 7);
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
