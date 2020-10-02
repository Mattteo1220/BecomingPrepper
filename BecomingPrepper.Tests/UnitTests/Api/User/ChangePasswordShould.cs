using System;
using AutoFixture;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.User
{
    [Trait("Unit", "ChangePassword")]
    public class ChangePasswordShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;

        public ChangePasswordShould()
        {
            _mockServiceAccount = new Mock<IServiceAccount>();
            _mockLogger = new Mock<ILogManager>();
            _userController = new UserController(_mockServiceAccount.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundIfNoAccountIdProvided()
        {
            _userController.ChangePassword(null, _fixture.Create<Authentication>()).Should().BeOfType<NotFoundResult>("No accountId provided");
        }

        [Fact]
        public void ReturnNotFoundIfNoAuthenticationProvided()
        {
            _userController.ChangePassword(_fixture.Create<string>(), null).Should().BeOfType<NotFoundResult>("No Authentication provided");
        }

        [Fact]
        public void ReturnOkWhenPasswordIsUpdatedSuccessfully()
        {
            _userController.ChangePassword(_fixture.Create<string>(), _fixture.Create<Authentication>()).Should().BeOfType<OkObjectResult>("The Password was updated.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockServiceAccount.Setup(sa => sa.UpdatePassword(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _userController.ChangePassword(_fixture.Create<string>(), _fixture.Create<Authentication>()).Should().BeOfType<NotFoundResult>("Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
