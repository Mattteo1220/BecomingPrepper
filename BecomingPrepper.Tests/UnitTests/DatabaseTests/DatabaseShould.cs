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
        private Action _invalidMongoClientAction;
        private readonly string MongoDatabase = "BecomingPrepper_Dev";
        private MongoClient _mongoClient;

        public DatabaseShould()
        {
            _mockConfiguration = TestHelper.GetMockConfiguration();
            _database = new Database();
            _mongoClient = _database.Connect(_mockConfiguration, MongoDatabase);
            _mockInvalidMongoClient = TestHelper.GetMockConfiguration(true);
            _invalidMongoClientAction = () => _database.Connect(_mockConfiguration, MongoDatabase);
        }

        [Fact]
        public void ReturnMongoClientOnConnect()
        {
            _mongoClient.Should().NotBe(null);
        }

        [Fact]
        public void BeAlive()
        {
            _database.IsAlive(_mongoClient, MongoDatabase).Should().Be(true);
        }

        [Fact]
        public void ThrowExceptionOnInvalidConnectionString()
        {
            _invalidMongoClientAction.Should().Throw<Exception>("The connection string was invalid.");
        }
    }
}
