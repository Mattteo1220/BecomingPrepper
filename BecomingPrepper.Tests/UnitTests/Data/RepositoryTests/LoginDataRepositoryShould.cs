using System;
using AutoFixture;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Data.RepositoryTests
{
    [Trait("Unit", "LoginDataRepositoryTests")]
    public class LoginDataRepositoryShould
    {
        private readonly Mock<IMongoDatabase> _mockMongoContext;
        private readonly Mock<ILogManager> _mockLogger;
        private Mock<IRepository<Login>> _mockLoginDataRepository;
        private readonly IRepository<Login> _loginDataRepository;
        private Fixture _fixture;
        public LoginDataRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockLogger = new Mock<ILogManager>();
            _mockLoginDataRepository = new Mock<IRepository<Login>>();
            _mockMongoContext = new Mock<IMongoDatabase>();
            _loginDataRepository = new LoginDataRepository(_mockMongoContext.Object, _mockLogger.Object);
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoMongoContextIsProvided()
        {
            Action nullMongoContext = () => new LoginDataRepository(null, _mockLogger.Object);
            nullMongoContext.Should().Throw<ArgumentNullException>("No Mongo Context was provided");
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoLogManagerIsProvided()
        {
            Action nullMongoContext = () => new LoginDataRepository(_mockMongoContext.Object, null);
            nullMongoContext.Should().Throw<ArgumentNullException>("No LogManager was provided");
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoLoginProvidedToAddMethod()
        {
            Action nullLogin = () => _loginDataRepository.Add(null);
            nullLogin.Should().Throw<ArgumentNullException>("No Login Was Provided");
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoFilterProvidedToGetMethod()
        {
            Action nullFilter = () => _loginDataRepository.Get(null);
            nullFilter.Should().Throw<ArgumentNullException>("No Filter was provided");
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoFilterIsProvidedToUpdateMethod()
        {
            Action nullFilter = () => _loginDataRepository.Update(null, Builders<Login>.Update.Set(l => l.LoginStamp, DateTime.Now));
            nullFilter.Should().Throw<ArgumentNullException>("No Query Filter was Provided");
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionWhenNoUpdateFilterIsProvidedToUpdateMethod()
        {
            Action nullFilter = () => _loginDataRepository.Update(Builders<Login>.Filter.Where(l => l.AccountId == _fixture.Create<string>()), null);
            nullFilter.Should().Throw<ArgumentNullException>("No Query Filter was Provided");
        }
    }
}
