using System;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class PrepGuideRepositoryShould
    {
        [Fact]
        public void ThrowIfNoMongoDatabaseSupplied()
        {
            //Arrange
            Action PrepGuideRepository;

            //Act
            PrepGuideRepository = () => new PrepGuideRepository(null, "PrepGuides");

            //Assert
            PrepGuideRepository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var prepGuideRepository = new PrepGuideRepository(mockDatabase.Object.MongoDatabase, "PrepGuides");
            prepGuideRepository.Dispose();

            //Asssert
            prepGuideRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
