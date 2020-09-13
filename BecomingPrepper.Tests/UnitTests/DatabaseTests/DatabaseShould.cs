using System;
using BecomingPrepper.Data.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using static FluentAssertions.AssertionExtensions;

namespace BecomingPrepper.Tests.UnitTests.DatabaseTests
{
    [Trait("Unit", "DatabaseTests")]
    public class DatabaseShould
    {
        private Mock<IDatabase> _mockDatabase;
        private IConfiguration _mockConfiguration;
        private IConfiguration _mockInvalidMongoClient;
        private const string Environment = "Dev";

        public DatabaseShould()
        {
            _mockDatabase = TestHelper.GetMockDatabase();
            _mockConfiguration = TestHelper.GetMockConfiguration();
        }

        [Fact]
        public void ReturnMongoClientWithDatabaseOnConnect()
        {
            _mockDatabase.Object.MongoClient.Should().NotBe(null);
        }

        [Fact]
        public void BeAlive()
        {
            _mockDatabase.Setup(db => db.IsAlive(_mockDatabase.Object.MongoClient, Environment)).Returns(true);
            _mockDatabase.Object.IsAlive(_mockDatabase.Object.MongoClient, Environment).Should().Be(true, "Correct MongoClient ConnectionString was provided.");
        }

        [Fact]
        public void ThrowExceptionOnInvalidConnectionString()
        {
            //Arrange
            _mockDatabase.Setup(db => db.Connect(_mockInvalidMongoClient, It.IsAny<string>())).Throws<ArgumentNullException>();

            //Act
            Action invalidMongoClientTestAction = () => _mockDatabase.Object.Connect(_mockInvalidMongoClient, Environment);

            //Assert
            invalidMongoClientTestAction.Should().Throw<Exception>("The connection string was invalid.");
        }

        [Fact]
        public void ThrowExceptionWhenConfigurationIsNull()
        {
            //Arrange
            _mockDatabase.Setup(db => db.Connect(null, It.IsAny<string>())).Throws<ArgumentNullException>();

            //Act
            Action nullConfigurationTestAction = () => _mockDatabase.Object.Connect(null, string.Empty);

            //Assert
            nullConfigurationTestAction.Should().Throw<ArgumentNullException>("Configuration was null.");
        }

        [Fact]
        public void ThrowExceptionWhenEnvironmentProvidedToConnectIsEmpty()
        {
            //Arrange
            _mockDatabase.Setup(db => db.Connect(_mockConfiguration, string.Empty)).Throws<ArgumentNullException>();

            //Act
            Action emptyEnvironmentTestAction = () => _mockDatabase.Object.Connect(_mockConfiguration, string.Empty);

            //Assert
            emptyEnvironmentTestAction.Should().Throw<ArgumentNullException>("Environment provided was Empty.");
        }

        [Fact]
        public void ThrowExceptionWhenMongoClientIsNull()
        {
            //Arrange
            _mockDatabase.Setup(db => db.IsAlive(null, It.IsAny<string>())).Throws<ArgumentNullException>();

            //Act
            Action nullMongoClientTestAction = () => _mockDatabase.Object.IsAlive(null, Environment);

            //Assert
            nullMongoClientTestAction.Should().Throw<ArgumentNullException>("MongoClient was null.");
        }

        [Fact]
        public void ThrowExceptionWhenDatabaseProvidedToIsAliveIsEmpty()
        {
            //Arrange
            _mockDatabase.Setup(db => db.IsAlive(_mockDatabase.Object.MongoClient, string.Empty)).Throws<ArgumentNullException>();

            //Act
            Action emptyDatabaseTestAction = () => _mockDatabase.Object.IsAlive(_mockDatabase.Object.MongoClient, string.Empty);

            //Assert
            emptyDatabaseTestAction.Should().Throw<ArgumentNullException>("Database provided was Empty.");
        }

        [Fact]
        public void InstantiateMongoDatabase_UponConnect()
        {
            _mockDatabase.Object.MongoDatabase.Should().NotBe(null);
        }
    }
}
