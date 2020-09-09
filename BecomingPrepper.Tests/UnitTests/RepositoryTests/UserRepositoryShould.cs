using System;
using System.Threading.Tasks;
using AutoFixture;
using BecomingPrepper.Data;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace BecomingPrepper.Tests.RepostioryTests.UnitTests
{
    [Trait("Unit", "UserRepositoryTests")]
    public class UserRepositoryShould
    {
        private IConfiguration _mockConfiguration;
        private IDatabase _database;
        private const string Environment = "Dev";
        private UserRepository _userRepository;
        private Action _initCollectionTest;
        private Func<Task> _nullUserEntityTest;
        private Func<Task> _getNullQueryFilterTest;
        private Func<Task> _updateQueryFilterTest;
        private Func<Task> _updateFilterTest;
        private Func<Task> _deleteFilterTest;
        public UserRepositoryShould()
        {
            _mockConfiguration = TestHelper.GetMockConfiguration();
            _database = new Database();
            _database.Connect(_mockConfiguration, Environment);
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _userRepository = new UserRepository();

            _userRepository.Init(_database.MongoDatabase);
            _initCollectionTest = () => _userRepository.Init(null);
            _nullUserEntityTest = async () => await _userRepository.Add(null);
            _getNullQueryFilterTest = async () => await _userRepository.Get(null);
            _updateQueryFilterTest = async () => await _userRepository.Update(null, Builders<UserEntity>.Update.Combine(Builders<UserEntity>.Update.Set(i => i._id, ObjectId.GenerateNewId())));
            _updateFilterTest = async () => await _userRepository.Update(Builders<UserEntity>.Filter.Empty, null);
            _deleteFilterTest = async () => await _userRepository.Delete(null);
        }

        [Fact]
        public void ThrowOnInit_IfNoMongoDatabaseParameterSupplied()
        {
            _initCollectionTest.Should().Throw<ArgumentNullException>("Because no 'MongoDatabase' was supplied'");
        }

        [Fact]
        public void InstantiateUserCollection()
        {
            using (new AssertionScope())
            {
                _userRepository.Init(_database.MongoDatabase).Should().BeAssignableTo<IMongoCollection<UserEntity>>();
                _userRepository.UserCollection.Should().NotBe(null, "Because it was instantiated");
            }
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
    }
}
