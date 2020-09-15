using System;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class RecommendedQuantityRepositoryShould
    {
        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action recommendedQuantityRespository;

            //Act
            recommendedQuantityRespository = () => new RecommendedQuantityRepository(null, "RecommendedQuantities");

            //Assert
            recommendedQuantityRespository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var recommendedQuantityRepository = new RecommendedQuantityRepository(mockDatabase.Object.MongoDatabase, "RecommendedQuantities");
            recommendedQuantityRepository.Dispose();

            //Asssert
            recommendedQuantityRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
