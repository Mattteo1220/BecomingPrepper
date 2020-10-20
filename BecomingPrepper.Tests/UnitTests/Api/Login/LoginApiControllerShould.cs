using System;
using AutoFixture;
using BecomingPrepper.Api.Authentication;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.TokenService.Interface;
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
        private Mock<ILoginUtility> _mockLogin;
        private LoginController _loginController;
        private Mock<ILogManager> _mockLogger;
        private Mock<ITokenManager> _mockTokenManager;
        private readonly Mock<ILoginDataService> _mockLoginDataService;
        private Fixture _fixture;
        public LoginApiControllerShould()
        {
            _mockLogin = new Mock<ILoginUtility>();
            _mockLogger = new Mock<ILogManager>();
            _mockTokenManager = new Mock<ITokenManager>();
            _mockLoginDataService = new Mock<ILoginDataService>();
            _loginController = new LoginController(_mockLogin.Object, _mockLoginDataService.Object, _mockLogger.Object, _mockTokenManager.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void ReturnNotFoundIfNoCredentialsSupplied()
        {
            _loginController.Login(null).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CallAuthenticateOnLogin()
        {
            _loginController.Login(_fixture.Create<Credentials>());
            _mockLogin.Verify(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ReturnUnAuthorizedWhenInvalidCredentialsAreSupplied()
        {
            _loginController.Login(_fixture.Create<Credentials>()).Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void ReturnOkWhenAuthorized()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _loginController.Login(_fixture.Create<Credentials>()).Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ReturnBearerTokenWhenAuthorized()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var response = _loginController.Login(_fixture.Create<Credentials>()) as OkObjectResult;
            response?.Value.ToString().Should().Contain("Token: ");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockLogin.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _loginController.Login(_fixture.Create<Credentials>()).Should().BeOfType<NotFoundResult>();
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
