﻿using System;
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
    public class PrepGuideRepositoryShould
    {
        private Mock<IMongoCollection<PrepGuideEntity>> _mockFoodStorageInventoryCollection;
        private Mock<ILogManager> _mockLogger;
        private Fixture _fixture;
        public PrepGuideRepositoryShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockFoodStorageInventoryCollection = new Mock<IMongoCollection<PrepGuideEntity>>();
            _mockLogger = new Mock<ILogManager>();
        }

        [Fact]
        public void ThrowIfNoMongoDatabaseSupplied()
        {
            //Arrange
            Action PrepGuideRepository;

            //Act
            PrepGuideRepository = () => new PrepGuideRepository(null, _mockLogger.Object);

            //Assert
            PrepGuideRepository.Should().Throw<ArgumentNullException>("No _collection was supplied.");
        }
    }
}
