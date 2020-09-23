using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.RepostioryTests.UnitTests
{
    [Trait("Unit", "UserRepositoryTests")]
    public class DatabaseCollectionShould
    {
        private IRepository<UserEntity> _userRepository;
        private Action _initCollectionTest;
        private Action _nullUserEntityTest;
        private Action _getNullQueryFilterTest;
        private Action _updateQueryFilterTest;
        private Action _updateFilterTest;
        private Action _deleteFilterTest;
        private Action _disposeTest;
        private Mock<IMongoCollection<UserEntity>> _mockUserCollection;
        private Mock<IExceptionLogger> _mockLogger;
        private Mock<IRepository<UserRepository>> _mockUserRepository;

        public DatabaseCollectionShould()
        {
            var mockDatabase = TestHelper.GetMockDatabase();
            var mockUserEntityCollection = new Mock<IMongoCollection<UserEntity>>();
            mockDatabase.Setup(db => db.MongoDatabase.GetCollection<UserEntity>(It.IsAny<string>(), null)).Returns(mockUserEntityCollection.Object);

            _mockUserCollection = new Mock<IMongoCollection<UserEntity>>();
            _mockLogger = new Mock<IExceptionLogger>();
            _mockUserRepository = new Mock<IRepository<UserRepository>>();
            _userRepository = new UserRepository(_mockUserCollection.Object, _mockLogger.Object);

            _initCollectionTest = () => new UserRepository(null, _mockLogger.Object);
            _nullUserEntityTest = () => _userRepository.Add(null);
            _getNullQueryFilterTest = () => _userRepository.Get(null);
            _updateQueryFilterTest = () => _userRepository.Update(null, Builders<UserEntity>.Update.Combine(Builders<UserEntity>.Update.Set(i => i._id, ObjectId.GenerateNewId())));
            _updateFilterTest = () => _userRepository.Update(Builders<UserEntity>.Filter.Empty, null);
            _deleteFilterTest = () => _userRepository.Delete(null);
            _disposeTest = () => _userRepository.Dispose();
        }

        [Fact]
        public void ThrowOnInit_IfNoMongoDatabaseParameterSupplied()
        {
            _initCollectionTest.Should().Throw<ArgumentNullException>("Because no 'MongoDatabase' was supplied'");
        }

        [Fact]
        public void InstantiateUserCollection()
        {
            _userRepository.Collection.Should().NotBe(null, "Because it was instantiated");
        }

        [Fact]
        public void ThrowOnAdd_WhenNoUserEntitySupplied()
        {
            _nullUserEntityTest.Should().Throw<ArgumentNullException>("Because no UserEntity was supplied");
        }

        [Fact]
        public void ThrowOnGet_WhenQueryFilterIsNull()
        {
            _getNullQueryFilterTest.Should().Throw<ArgumentNullException>("Because No Query filter was provided.");
        }

        [Fact]
        public void ThrowOnUpdate_WhenQueryFilterIsNull()
        {
            _updateQueryFilterTest.Should().Throw<ArgumentNullException>("Because No Query filter was provided.");
        }

        [Fact]
        public void ThrowOnUpdate_WhenUpdateFilterIsNull()
        {
            _updateFilterTest.Should().Throw<ArgumentNullException>("Because No Update filter was provided.");
        }

        [Fact]
        public void ThrowOnDelete_WhenQueryFilterIsNull()
        {
            _deleteFilterTest.Should().Throw<ArgumentNullException>("Because No Query filter was provided.");
        }

        [Fact]
        public void DisposeOfCollection()
        {
            _disposeTest.Invoke();
            _userRepository.Collection.Should().BeNull("We disposed of the database connection");
        }
    }
}
