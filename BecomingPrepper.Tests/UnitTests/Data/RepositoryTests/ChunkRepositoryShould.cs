using System;
using AutoFixture;
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
    [Trait("Unit", "ChunkRepositoryTests")]
    public class ChunkRepositoryShould
    {
        private IChunkRepository _chunkRepository;
        private Mock<IMongoDatabase> _mockMongoContext;
        private Mock<ILogManager> _mockLogManager;
        private IMongoDatabase _mongoContext;
        private Fixture _fixture;
        public ChunkRepositoryShould()
        {
            _mockMongoContext = new Mock<IMongoDatabase>();
            _mockLogManager = new Mock<ILogManager>();
            _chunkRepository = new ChunkRepository(_mockMongoContext.Object, _mockLogManager.Object);
            _mongoContext = TestHelper.GetDatabase();
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoIMongoContextProvided()
        {
            Action nullArgumentTest = () => new ChunkRepository(null, _mockLogManager.Object);
            nullArgumentTest.Should().Throw<ArgumentNullException>("No Mongo Context was provided");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoILogManagerProvided()
        {
            Action nullArgumentTest = () => new ChunkRepository(_mongoContext, null);
            nullArgumentTest.Should().Throw<ArgumentNullException>("No LogManager was provided");
        }

        [Fact]
        public void ThrowWhenNoFilesIdIsProvided()
        {
            Action emptyObjectId = () => _chunkRepository.GetInventoryImageChunksAsync(ObjectId.Empty).Wait();
            emptyObjectId.Should().Throw<ArgumentNullException>("No ObjectId filesId was provided");
        }
    }
}
