using System;
using AutoFixture;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class RecommendedQuantityRepositoryShould
    {
        private Mock<IMongoDatabase> _mockMongoContext;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public RecommendedQuantityRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockMongoContext = new Mock<IMongoDatabase>();
            _mockLogger = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action recommendedQuantityRespository;

            //Act
            recommendedQuantityRespository = () => new RecommendedQuantityRepository(null, _mockLogger.Object);

            //Assert
            recommendedQuantityRespository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }
    }
}
