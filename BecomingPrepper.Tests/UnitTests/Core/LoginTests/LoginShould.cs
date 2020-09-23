﻿using System;
using System.Security;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.LoginTests
{
    [Trait("Unit", "LoginUser")]
    public class LoginShould
    {
        private ILogin _login;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public LoginShould()
        {
            _login = Mock.Of<ILogin>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
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
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns((UserEntity)null);
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);

            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>()).Should().BeFalse("Nothing was returned from the GetUser Method");
        }

        [Fact]
        public void CallLogInformation_WhenNeedsUpgradingIsTrue()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _mockSecureService.Setup(ss => ss.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns((false, true));
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(el => el.LogWarning(It.IsAny<SecurityException>()), Times.Once);
        }

        [Fact]
        public void CallGetUserFromDatabase()
        {
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>());
            _mockUserRepo.Verify(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasBeenVerified()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(TestHelper.GenerateUserEntity);
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogInformation_WhenUserHasNotBeenVerified()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Throws<Exception>();
            _login = new Login(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _login.Authenticate(_fixture.Create<string>(), _fixture.Create<string>());
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}