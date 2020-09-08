using System;
using BecomingPrepper.Data;
using BecomingPrepper.Data.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.DatabaseTests
{
    [Trait("Unit", "DatabaseTests")]
    public class DatabaseShould
    {
        private IConfiguration _mockConfiguration;
        private IDatabase _database;
        private IConfiguration _mockInvalidMongoClient;
        private const string MongoDatabase = "Dev";
        private MongoClient _mongoClient;

        private Action _invalidMongoClientTestAction;
        private Action _nullConfigurationTestAction;
        private Action _emptyEnvironmentTestAction;
        private Action _nullMongoClientTestAction;
        private Action _emptyDatabaseTestAction;

        public DatabaseShould()
        {
            _mockConfiguration = TestHelper.GetMockConfiguration();
            _database = new Database();
            _mongoClient = _database.Connect(_mockConfiguration, MongoDatabase);
            _mockInvalidMongoClient = TestHelper.GetMockConfiguration(true);
            _invalidMongoClientTestAction = () => _database.Connect(_mockInvalidMongoClient, MongoDatabase);
            _nullConfigurationTestAction = () => _database.Connect(null, MongoDatabase);
            _emptyEnvironmentTestAction = () => _database.Connect(_mockConfiguration, string.Empty);
            _nullMongoClientTestAction = () => _database.IsAlive(null, MongoDatabase);
            _emptyDatabaseTestAction = () => _database.IsAlive(_mongoClient, string.Empty);
        }

        [Fact]
        public void ReturnMongoClientOnConnect()
        {
            _mongoClient.Should().NotBe(null);
        }

        [Fact]
        public void BeAlive()
        {
            _database.IsAlive(_mongoClient, MongoDatabase).Should().Be(true, "Correct MongoClient ConnectionString was provided.");
        }

        [Fact]
        public void ThrowExceptionOnInvalidConnectionString()
        {
            _invalidMongoClientTestAction.Should().Throw<Exception>("The connection string was invalid.");
        }

        [Fact]
        public void ThrowExceptionWhenConfigurationIsNull()
        {
            _nullConfigurationTestAction.Should().Throw<ArgumentNullException>("Configuration was null.");
        }

        [Fact]
        public void ThrowExceptionWhenEnvironmentProvidedToConnectIsEmpty()
        {
            _emptyEnvironmentTestAction.Should().Throw<ArgumentNullException>("Environment provided was Empty.");
        }

        [Fact]
        public void ThrowExceptionWhenMongoClientIsNull()
        {
            _nullMongoClientTestAction.Should().Throw<ArgumentNullException>("MongoClient was null.");
        }

        [Fact]
        public void ThrowExceptionWhenDatabaseProvidedToIsAliveIsEmpty()
        {
            _emptyDatabaseTestAction.Should().Throw<ArgumentNullException>("Database provided was Empty.");
        }
    }
}
