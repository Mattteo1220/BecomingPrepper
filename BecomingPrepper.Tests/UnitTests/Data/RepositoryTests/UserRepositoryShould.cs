using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class UserRepositoryShould
    {
        private Mock<IMongoCollection<UserEntity>> _mockUserRepo;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public UserRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockUserRepo = new Mock<IMongoCollection<UserEntity>>();
            _mockLogger = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action userRepository;

            //Act
            userRepository = () => new UserRepository(null, _mockLogger.Object);

            //Assert
            userRepository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var userRepository = new UserRepository(_mockUserRepo.Object, _mockLogger.Object);
            userRepository.Dispose();

            //Asssert
            userRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
