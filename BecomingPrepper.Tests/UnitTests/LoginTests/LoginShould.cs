using System;
using System.Security;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.LoginTests
{
    public class LoginShould
    {
        private ILogin _login;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<IExceptionLogger> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public LoginShould()
        {
            _login = Mock.Of<ILogin>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<IExceptionLogger>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(" ", "lskdfisdkd")]
        [InlineData("lskdjfjsdf", null)]
        public void Throw_WhenInvalidCredentialsSupplied(string username, string password)
        {
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            Action invalidParametersTest = () => _login.Authenticate(username, password);
            invalidParametersTest.Should().Throw<ArgumentNullException>("Invalid Parameters Supplied");
        }

        [Fact]
        public void FailToAuthenticate_WhenNullReturnedFromUserRepository()
        {
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns((UserEntity)null);
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);

            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeFalse("Nothing was returned from the GetUser Method");
        }

        [Fact]
        public void LogNeedsUpgrading_WhenNeedsUpgradingIsTrue()
        {
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(GenerateUserEntity);
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, true));
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(el => el.LogWarning(It.IsAny<SecurityException>()), Times.Once);
        }

        #region helperMethods

        private UserEntity GenerateUserEntity()
        {
            _fixture.Register(ObjectId.GenerateNewId);
            return _fixture.Create<UserEntity>();
        }
        #endregion

    }
}
