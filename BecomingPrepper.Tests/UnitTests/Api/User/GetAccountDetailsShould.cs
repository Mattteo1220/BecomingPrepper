using System;
using AutoFixture;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.User
{
    [Trait("Unit", "GetAccountDetails")]
    public class GetAccountDetailsShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;

        public GetAccountDetailsShould()
        {
            _mockServiceAccount = new Mock<IServiceAccount>();
            _mockLogger = new Mock<ILogManager>();
            _userController = new UserController(_mockServiceAccount.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundWhenNoAccountIdSupplied()
        {
            _userController.GetAccountDetails(" ").Should().BeOfType<NotFoundResult>("No AccountId was supplied");
        }

        [Fact]
        public void ReturnOkWhenAccountDetailsReturned()
        {
            _mockServiceAccount.Setup(sa => sa.GetAccountDetails(It.IsAny<string>())).Returns(_fixture.Create<UserEntity>());
            _userController.GetAccountDetails(_fixture.Create<string>()).Should().BeOfType<OkObjectResult>("Account Details should have been returned.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockServiceAccount.Setup(sa => sa.GetAccountDetails(It.IsAny<string>())).Throws<Exception>();
            _userController.GetAccountDetails(_fixture.Create<string>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public void ReturnNoContentWhenNothingIsReturned()
        {
            _userController.GetAccountDetails(_fixture.Create<string>()).Should().BeOfType<NoContentResult>("No user Account Details was returned");
        }
    }
}
