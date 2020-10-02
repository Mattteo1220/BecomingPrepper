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
    [Trait("Unit", "PatchFamilySize")]
    public class PatchFamilySizeShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;

        public PatchFamilySizeShould()
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
            _userController.PatchFamilySize(string.Empty, _fixture.Create<Family>()).Should().BeOfType<NotFoundResult>("No accountId was provided");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ReturnNotFoundWhenFamilySizeIsLessThanOrEqualToZero(int familySize)
        {
            var family = new Family() { Size = familySize };
            _userController.PatchFamilySize(_fixture.Create<string>(), family).Should().BeOfType<BadRequestObjectResult>("Family size is not supported");
        }

        [Fact]
        public void ReturnOkWhenUpdatingFamilySizeIsSuccessful()
        {
            _userController.PatchFamilySize(_fixture.Create<string>(), _fixture.Create<Family>()).Should().BeOfType<OkObjectResult>("updating the familySize was successful");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockServiceAccount.Setup(sa => sa.UpdateFamilySize(It.IsAny<string>(), It.IsAny<int>())).Throws<Exception>();
            _userController.PatchFamilySize(_fixture.Create<string>(), _fixture.Create<Family>()).Should().BeOfType<NotFoundResult>("An Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
