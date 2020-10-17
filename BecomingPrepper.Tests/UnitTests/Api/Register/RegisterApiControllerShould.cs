using System;
using AutoFixture;
using AutoMapper;
using BecomingPrepper.Api.Controllers.User;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Logger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Api.Register
{
    public class RegisterApiControllerShould
    {
        private Mock<IRegister> _mockRegister;
        private RegisterController _registerController;
        private Mock<ILogManager> _mockLogger;
        private Mock<IMapper> _mockMapper;
        private Fixture _fixture;
        public RegisterApiControllerShould()
        {
            _mockRegister = new Mock<IRegister>();
            _mockLogger = new Mock<ILogManager>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowOnInitIfNoIRegistrationSupplied()
        {
            Action nullParameter = () => new RegisterController(null, _mockMapper.Object, _mockLogger.Object);
            nullParameter.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowOnInitIfNoIMapperSupplied()
        {
            Action nullParameter = () => new RegisterController(_mockRegister.Object, null, _mockLogger.Object);
            nullParameter.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowOnInitIfNoILoggerSupplied()
        {
            Action nullParameter = () => new RegisterController(_mockRegister.Object, _mockMapper.Object, null);
            nullParameter.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReturnNotFoundIfNoUserRegistrationIfNotSupplied()
        {
            _registerController = new RegisterController(_mockRegister.Object, _mockMapper.Object, _mockLogger.Object);
            _registerController.Register(null).Should().BeOfType<NotFoundResult>("No UserRegistration was supplied");
        }

        [Fact]
        public void ReturnOkWhenTheUserHasBeenRegistered()
        {
            _mockMapper.Setup(m => m.Map<UserEntity>(It.IsAny<UserRegistrationInfo>())).Returns(_fixture.Create<UserEntity>());
            _registerController = new RegisterController(_mockRegister.Object, _mockMapper.Object, _mockLogger.Object);
            _registerController.Register(_fixture.Create<UserRegistrationInfo>()).Should().BeOfType<OkResult>("The user was registered successfully.");
        }

        [Fact]
        public void ReturnNotFoundWhenUsernameAlreadyInUseIsSupplied()
        {
            _mockMapper.Setup(m => m.Map<UserEntity>(It.IsAny<UserRegistrationInfo>())).Returns(_fixture.Create<UserEntity>());
            _mockRegister.Setup(r => r.Register(It.IsAny<UserEntity>())).Throws<InvalidOperationException>();
            _registerController = new RegisterController(_mockRegister.Object, _mockMapper.Object, _mockLogger.Object);
            _registerController.Register(_fixture.Create<UserRegistrationInfo>()).Should().BeOfType<NotFoundObjectResult>("A username already in use was supplied.");
        }

        [Fact]
        public void ReturnNotFoundWhenExceptionIsThrown()
        {
            _mockMapper.Setup(m => m.Map<UserEntity>(It.IsAny<UserRegistrationInfo>())).Returns(_fixture.Create<UserEntity>());
            _mockRegister.Setup(r => r.Register(It.IsAny<UserEntity>())).Throws<Exception>();
            _registerController = new RegisterController(_mockRegister.Object, _mockMapper.Object, _mockLogger.Object);
            _registerController.Register(_fixture.Create<UserRegistrationInfo>()).Should().BeOfType<NotFoundObjectResult>("An Exception was thrown");
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>()), Times.Once);
        }
    }
}
