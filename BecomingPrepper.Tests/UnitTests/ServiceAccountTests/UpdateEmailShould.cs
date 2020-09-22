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
    [Trait("Unit", "UpdateEmailTests")]
    public class UpdateEmailShould
    {
        private IServiceAccount _serviceAccount;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<IExceptionLogger> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public UpdateEmailShould()
        {
            _serviceAccount = Mock.Of<IServiceAccount>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<IExceptionLogger>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
        }

        [Theory]
        [InlineData(" ", "something@gmail.com")]
        [InlineData(null, "Something@Gmail.com")]
        [InlineData("sldkjflkjs", "")]
        [InlineData("ieidjdls1234", null)]
        public void Throw_WhenNoEmailIsSupplied(string accountId, string email)
        {
            Action invalidEmailTest = () => _serviceAccount.UpdateEmail(accountId, email);
            invalidEmailTest.Should().Throw<ArgumentNullException>("No Valid Email was supplied");
        }

        [Fact]
        public void CallUpdateInUserRepository()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateEmail(_fixture.Create<string>(), _fixture.Create<string>());
            _mockUserRepo.Verify(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasUpdatedTheirEmail()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>()));

            _serviceAccount.UpdateEmail(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformation_WhenUpdateHasThrownAnException()
        {
            _mockUserRepo.Setup(ur => ur.Update(It.IsAny<FilterDefinition<UserEntity>>(), It.IsAny<UpdateDefinition<UserEntity>>())).Throws<Exception>();

            _serviceAccount.UpdateEmail(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(ur => ur.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
