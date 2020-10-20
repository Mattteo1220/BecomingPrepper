using System;
using AutoFixture;
using BecomingPrepper.Security;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.SecureServiceTests
{
    [Trait("Unit", "SecurityHashTests")]
    public class SecureServiceShould
    {
        private ISecureService _secureService;
        private Fixture _fixture;
        public SecureServiceShould()
        {
            _fixture = new Fixture();
            _secureService = new Security.SecureService(new HashingOptions());
        }

        [Fact]
        public void Hash_PasswordWithThreeDistinctParts()
        {
            var hashedPassword = _secureService.Hash(_fixture.Create<string>());
            var parts = hashedPassword.Split(".");
            parts.Length.Should().Be(3, "The hashing algorithm hashes them into three distinct parts.");
        }

        [Fact]
        public void Check_CorrectPassword_Verified()
        {
            var password = _fixture.Create<string>();
            var hashedPassword = _secureService.Hash(password);
            var result = _secureService.Validate(hashedPassword, password);
            result.Verified.Should().Be(true, "correct password was supplied");
        }

        [Fact]
        public void Check_InCorrectPassword_NotVerified()
        {
            var hashedPassword = _secureService.Hash(_fixture.Create<string>());
            var result = _secureService.Validate(hashedPassword, _fixture.Create<string>());
            result.Verified.Should().Be(false, "Incorrect password was supplied");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void Hash_InvalidPasswordSupplied_ThrowsArgumentNullException(string password)
        {
            //Arrange
            Action invalidPasswordTest = () => _secureService.Hash(password);
            invalidPasswordTest.Should().Throw<ArgumentNullException>("Invalid Password supplied.");
        }

        [Theory]
        [InlineData(" ", "qwerty")]
        [InlineData("sldkfowielslksldf=!@#$", null)]
        public void Validate_InvalidArgumentsSupplied_ThrowsArgumentNullException(string hash, string password)
        {
            //Arrange
            Action invalidHashArgumentsTest = () => _secureService.Validate(hash, password);
            invalidHashArgumentsTest.Should().Throw<ArgumentNullException>("Invalid Arguments supplied.");
        }
    }
}
