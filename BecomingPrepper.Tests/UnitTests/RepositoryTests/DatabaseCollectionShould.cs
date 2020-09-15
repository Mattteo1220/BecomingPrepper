using System;
using System.Threading.Tasks;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
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
        private Func<Task> _nullUserEntityTest;
        private Func<Task> _getNullQueryFilterTest;
        private Func<Task> _updateQueryFilterTest;
        private Func<Task> _updateFilterTest;
        private Func<Task> _deleteFilterTest;
        private Action _disposeTest;
        public DatabaseCollectionShould()
        {
            var mockDatabase = TestHelper.GetMockDatabase();
            var mockUserEntityCollection = new Mock<IMongoCollection<UserEntity>>();
            mockDatabase.Setup(db => db.MongoDatabase.GetCollection<UserEntity>(It.IsAny<string>(), null)).Returns(mockUserEntityCollection.Object);

            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _userRepository = new UserRepository(mockDatabase.Object.MongoDatabase, "Users");

            _initCollectionTest = () => new UserRepository(null, "Users");
            _nullUserEntityTest = async () => await _userRepository.Add(null);
            _getNullQueryFilterTest = async () => await _userRepository.Get(null);
            _updateQueryFilterTest = async () => await _userRepository.Update(null, Builders<UserEntity>.Update.Combine(Builders<UserEntity>.Update.Set(i => i._id, ObjectId.GenerateNewId())));
            _updateFilterTest = async () => await _userRepository.Update(Builders<UserEntity>.Filter.Empty, null);
            _deleteFilterTest = async () => await _userRepository.Delete(null);
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
