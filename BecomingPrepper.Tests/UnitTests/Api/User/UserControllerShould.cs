using System;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Logger;
using FluentAssertions;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.User
{
    [Trait("Unit", "UserControllerCTOR")]
    public class UserControllerShould
    {
        private Mock<IServiceAccount> _mockServiceAccount;
        private UserController _userController;
        private Mock<ILogManager> _mockLogger;
        public UserControllerShould()
        {
            _mockServiceAccount = new Mock<IServiceAccount>();
            _mockLogger = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoServiceAccountSupplied()
        {
            Action errorTest = () => new UserController(null, _mockLogger.Object);
            errorTest.Should().Throw<ArgumentNullException>("Required Parameter IServiceAccount was not supplied");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoILogManagerSupplied()
        {
            Action errorTest = () => new UserController(_mockServiceAccount.Object, null);
            errorTest.Should().Throw<ArgumentNullException>("Required Parameter ILogManager was not supplied");
        }
    }
}
