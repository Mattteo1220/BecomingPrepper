using System;
using AutoFixture;
using BecomingPrepper.Core.GridFSHelper;
using BecomingPrepper.Core.ImageResourceHelper;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Core.GridFSHelperTests
{
    [Trait("Unit", "GridFSHelperTests")]
    public class GridFSHelperShould
    {
        private IGridFSHelper _gridFsHelper;
        private Mock<IFileDetailRepository> _mockFileDetailRepo;
        private Mock<IChunkRepository> _mockChunkRepo;
        private Fixture _fixture;
        public GridFSHelperShould()
        {
            _mockChunkRepo = new Mock<IChunkRepository>();
            _mockFileDetailRepo = new Mock<IFileDetailRepository>();
            _gridFsHelper = new GridFSHelper(_mockFileDetailRepo.Object, _mockChunkRepo.Object);
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Fact]
        public void ThrowWhenNoConstructorFileDetailRepoIsProvided()
        {
            Action nullIFileDetailRepo = () => new GridFSHelper(null, _mockChunkRepo.Object);
            nullIFileDetailRepo.Should().Throw<ArgumentNullException>("No FileDetailRepo was provided");
        }

        [Fact]
        public void ThrowWhenNoConstructorChunkRepoIsProvided()
        {
            Action nullIFileDetailRepo = () => new GridFSHelper(_mockFileDetailRepo.Object, null);
            nullIFileDetailRepo.Should().Throw<ArgumentNullException>("No ChunkRepo was provided");
        }
    }
}
