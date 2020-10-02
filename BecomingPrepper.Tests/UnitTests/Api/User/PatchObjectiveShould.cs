using System;
using System.Linq;
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
    public class PatchObjectiveShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;

        public PatchObjectiveShould()
        {
            _mockServiceAccount = new Mock<IServiceAccount>();
            _mockLogger = new Mock<ILogManager>();
            _userController = new UserController(_mockServiceAccount.Object, _mockLogger.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ReturnNotFoundIfNoAccountIdIsProvided()
        {
            _userController.PatchObjective(string.Empty, _fixture.Create<Scheme>()).Should().BeOfType<NotFoundResult>("No accountId was provided");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(11)]
        public void ReturnBadRequestWhenInvalidObjectiveProvided(int objective)
        {
            var scheme = new Scheme() { Objective = objective };
            _userController.PatchObjective(_fixture.Create<string>(), scheme).Should().BeOfType<BadRequestObjectResult>("Objective was successfully updated.");
        }

        [Fact]
        public void ReturnOkWhenObjectiveIsUpdatedSuccessful()
        {
            var value = _fixture.Create<Generator<int>>().Where(x => x >= 1 && x <= 9);
            var scheme = new Scheme() { Objective = value.FirstOrDefault() };
            _userController.PatchObjective(_fixture.Create<string>(), scheme).Should().BeOfType<OkObjectResult>("Objective was Updated.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionThrown()
        {
            _mockServiceAccount.Setup(sa => sa.UpdateObjective(It.IsAny<string>(), It.IsAny<int>())).Throws<Exception>();
            var value = _fixture.Create<Generator<int>>().Where(x => x >= 1 && x <= 9);
            var scheme = new Scheme() { Objective = value.FirstOrDefault() };
            _userController.PatchObjective(_fixture.Create<string>(), scheme).Should().BeOfType<NotFoundResult>("Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);

        }
    }
}
