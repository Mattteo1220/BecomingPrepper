using System;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Enums;
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
        private Mock<ILogManager> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public UpdateObjectiveShould()
        {
            _serviceAccount = Mock.Of<IServiceAccount>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowWhenNoValidAccountIdSupplied(string accountId)
        {
            Action invalidAccountIdTest = () => _serviceAccount.UpdateObjective(accountId, Objective.TwoWeek);
            invalidAccountIdTest.Should().Throw<ArgumentNullException>("No Valid AccountId was supplied");
        }

        [Fact]
        public void CallUpdateInUserRepository()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateObjective(_fixture.Create<string>(), Objective.ThreeMonth);
            _mockUserRepo.Verify(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasUpdatedTheirEmail()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateObjective(_fixture.Create<string>(), Objective.OneYear);
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformation_WhenUpdateHasThrownAnException()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();

            Action errorTest = () => _serviceAccount.UpdateObjective(_fixture.Create<string>(), Objective.FiveYear);
            errorTest.Should().Throw<Exception>();
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
