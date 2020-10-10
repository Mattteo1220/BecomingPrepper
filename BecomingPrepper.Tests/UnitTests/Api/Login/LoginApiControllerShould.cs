﻿using System;
using AutoFixture;
using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.Login
{
    public class LoginApiControllerShould
    {
        private Mock<ILogin> _mockLogin;
        private LoginController _loginController;
        private Mock<ILogManager> _mockLogger;
        private Mock<ITokenManager> _mockTokenManager;
        private Fixture _fixture;
        public LoginApiControllerShould()
        {
            _mockLogin = new Mock<ILogin>();
            _mockLogger = new Mock<ILogManager>();
            _mockTokenManager = new Mock<ITokenManager>();
            _loginController = new LoginController(_mockLogin.Object, _mockLogger.Object, _mockTokenManager.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void ReturnNotFoundIfNoCredentialsSupplied()
        {
            _loginController.Post(null).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CallAuthenticateOnLogin()
        {
            _loginController.Post(_fixture.Create<Credentials>());
            _mockLogin.Verify(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ReturnUnAuthorizedWhenInvalidCredentialsAreSupplied()
        {
            _loginController.Post(_fixture.Create<Credentials>()).Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void ReturnOkWhenAuthorized()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _loginController.Post(_fixture.Create<Credentials>()).Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ReturnBearerTokenWhenAuthorized()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var response = _loginController.Post(_fixture.Create<Credentials>()) as OkObjectResult;
            response?.Value.ToString().Should().Contain("Token: ");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _loginController.Post(_fixture.Create<Credentials>()).Should().BeOfType<NotFoundResult>();
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}