using System;
using AutoFixture;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RegisterTests
{
    [Trait("Unit", "RegisterUser")]
    public class RegisterShould
    {
        private IRegister _register;
        private Mock<IRepository<UserEntity>> _mockUserRepo;
        private Mock<ILogManager> _mockExceptionLogger;
        private Mock<ISecureService> _mockSecureService;
        private Fixture _fixture;
        public RegisterShould()
        {
            _register = Mock.Of<IRegister>();
            _mockUserRepo = new Mock<IRepository<UserEntity>>();
            _mockExceptionLogger = new Mock<ILogManager>();
            _mockSecureService = new Mock<ISecureService>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void Throw_WhenNoEntitySupplied()
        {
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            Action nullEntityTest = () => _register.Register(null);

            nullEntityTest.Should().Throw<ArgumentNullException>("No Entity was supplied");
        }

        [Fact]
        public void CallHashPassword()
        {
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _register.Register(_fixture.Create<UserEntity>());
            _mockSecureService.Verify(ss => ss.Hash(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void CallAddToDatabase()
        {
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _register.Register(_fixture.Create<UserEntity>());
            _mockUserRepo.Verify(ur => ur.Add(It.IsAny<UserEntity>()), Times.Once);
        }

        [Fact]
        public void CallLogInformation_WhenUserHasBeenRegistered()
        {
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            _register.Register(_fixture.Create<UserEntity>());
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotCallLogUser_WhenUserRepositoryGetHasThrownAnException()
        {
            _mockUserRepo.Setup(ur => ur.Add(It.IsAny<UserEntity>())).Throws<Exception>();
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            Action errorTest = () => _register.Register(_fixture.Create<UserEntity>());
            errorTest.Should().Throw<Exception>();
            _mockExceptionLogger.Verify(el => el.LogInformation(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ThrowAnInvalidOperationExceptionWhenGetReturnsAUserThatHasAUsernameAlreadyInUse()
        {
            _mockUserRepo.Setup(ur => ur.Get(It.IsAny<FilterDefinition<UserEntity>>())).Returns(_fixture.Create<UserEntity>());
            _register = new RegisterService(_mockUserRepo.Object, _mockSecureService.Object, _mockExceptionLogger.Object);
            Action errorTest = () => _register.Register(_fixture.Create<UserEntity>());
            errorTest.Should().Throw<InvalidOperationException>("A user with a username already in use was returned");
        }
    }
}
