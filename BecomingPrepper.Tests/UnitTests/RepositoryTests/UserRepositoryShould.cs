using System;
using BecomingPrepper.Data.Repositories;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.RepositoryTests
{
    public class UserRepositoryShould
    {
        [Fact]
        public void ThrowIfNoMongoDatabaseIsSupplied()
        {
            //Arrange
            Action userRepository;

            //Act
            userRepository = () => new UserRepository(null);

            //Assert
            userRepository.Should().Throw<ArgumentNullException>("No IMongo database was supplied.");
        }

        [Fact]
        public void DisposeProperly()
        {
            //Arrrange
            var mockDatabase = TestHelper.GetMockDatabase();

            //Act
            var userRepository = new UserRepository(mockDatabase.Object.MongoDatabase);
            userRepository.Dispose();

            //Asssert
            userRepository.Collection.Should().BeNull("It was disposed of");
        }
    }
}
