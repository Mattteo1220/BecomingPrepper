using System;
using AutoFixture;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Data.RepositoryTests
{
    [Trait("Unit", "FileDetailRepoTests")]
    public class FileDetailRepositoryShould
    {
        private IFileDetailRepository _fileDetailRepo;
        private Mock<ILogManager> _mockLogManager;
        private Mock<IMongoDatabase> _mockMongoContext;
        private IMongoDatabase _mongoContext;
        private Fixture _fixture;
        public FileDetailRepositoryShould()
        {
            _mockMongoContext = new Mock<IMongoDatabase>();
            _mockLogManager = new Mock<ILogManager>();
            _mongoContext = TestHelper.GetDatabase();
            _fileDetailRepo = new FileDetailRepository(_mongoContext, _mockLogManager.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoIMongoContextProvided()
        {
            Action nullArgumentTest = () => new FileDetailRepository(null, _mockLogManager.Object);
            nullArgumentTest.Should().Throw<ArgumentNullException>("No Mongo Context was provided");
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenNoILogManagerProvided()
        {
            Action nullArgumentTest = () => new ChunkRepository(_mongoContext, null);
            nullArgumentTest.Should().Throw<ArgumentNullException>("No LogManager was provided");
        }

        [Fact]
        public void ThrowWhenNoItemIdIsProvided()
        {
            Action emptyItemId = () => _fileDetailRepo.GetInventoryImageFilesAsync(string.Empty).Wait();
            emptyItemId.Should().Throw<ArgumentNullException>("No ObjectId filesId was provided");
        }
    }
}
