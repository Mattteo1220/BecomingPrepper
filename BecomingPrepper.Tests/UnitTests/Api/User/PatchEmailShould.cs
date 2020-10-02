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
    [Trait("Unit", "PatchEmail")]
    public class PatchEmailShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;

        public PatchEmailShould()
        {
            _mockServiceAccount = new Mock<IServiceAccount>();
            _mockLogger = new Mock<ILogManager>();
            _userController = new UserController(_mockServiceAccount.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdProvided()
        {
            _userController.PatchEmail(string.Empty, _fixture.Create<ECommunication>()).Should().BeOfType<NotFoundResult>("No accountId was provided");
        }

        [Fact]
        public void ReturnNotFoundWhenNoECommunicationProvided()
        {
            _userController.PatchEmail(_fixture.Create<string>(), null).Should().BeOfType<NotFoundResult>("No ECommunication was provided");
        }

        [Fact]
        public void ReturnOkWhenEmailHasSuccessfullyBeenUpdated()
        {
            _userController.PatchEmail(_fixture.Create<string>(), _fixture.Create<ECommunication>()).Should().BeOfType<OkObjectResult>("Email was supposed to be updated.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockServiceAccount.Setup(sa => sa.UpdateEmail(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _userController.PatchEmail(_fixture.Create<string>(), _fixture.Create<ECommunication>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
        }
    }
}
