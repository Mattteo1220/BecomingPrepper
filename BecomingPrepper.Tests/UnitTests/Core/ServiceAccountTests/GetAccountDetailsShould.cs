using System;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.ServiceAccountTests
{
    [Trait("Unit", "GetAccountDetails")]
    public class GetAccountDetailsShould
    {
        private IServiceAccount _serviceAccount;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public GetAccountDetailsShould()
        {
            _serviceAccount = Mock.Of<IServiceAccount>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoParameterSupplied()
        {
            _serviceAccount = new ServiceAccount(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            Action error = () => _serviceAccount.GetAccountDetails(null);
            error.Should().Throw<ArgumentNullException>("No accountId was provided");
        }
    }
}
