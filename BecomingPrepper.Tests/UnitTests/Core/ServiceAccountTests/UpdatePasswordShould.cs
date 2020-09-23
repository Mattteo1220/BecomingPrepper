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
    [Trait("Unit", "UpdatePasswordTests")]
    public class UpdatePasswordShould
    {
        private IServiceAccount _serviceAccount;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public UpdatePasswordShould()
        {
            _serviceAccount = Mock.Of<IServiceAccount>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
        }

        [Theory]
        [InlineData(" ", "something")]
        [InlineData(null, "Something")]
        [InlineData("sldkjflkjs", "")]
        [InlineData("ieidjdls1234", null)]
        public void Throw_WhenNoValidArgumentsSupplied(string accountId, string password)
        {
            Action invalidEmailTest = () => _serviceAccount.UpdatePassword(accountId, password);
            invalidEmailTest.Should().Throw<ArgumentNullException>("No Valid arguments were supplied");
        }

        [Fact]
        public void CallUpdateInUserRepository()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, false));

            _serviceAccount.UpdatePassword(_fixture.Create<string>(), _fixture.Create<string>());
            _mockUserRepo.Verify(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasUpdatedTheirPassword()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, false));

            _serviceAccount.UpdatePassword(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformation_WhenUpdateHasThrownAnException()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, false));

            _serviceAccount.UpdatePassword(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void NotCallUpdate_WhenValidateVerifiedReturnsTrue()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((true, false));

            _serviceAccount.UpdatePassword(_fixture.Create<string>(), _fixture.Create<string>());
            _mockSecureService.Verify(ss => ss.Hash(It.IsAny<string>()), Times.Never);
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Never);
            _serviceAccount.Match.HasError.Should().BeTrue("Verified returned true");
            _serviceAccount.Match.Message.Should().Contain("Reuse of passwords is forbidden", "Verified returned True");
        }

        [Fact]
        public void CallHash_WhenUpdatingPassword()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, false));

            _serviceAccount.UpdatePassword(_fixture.Create<string>(), _fixture.Create<string>());
            _mockSecureService.Verify(ss => ss.Hash(It.IsAny<string>()), Times.Once);
        }
    }
}
