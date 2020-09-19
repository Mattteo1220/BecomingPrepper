using BecomingPrepper.Security;
using FluentAssertions;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests
{
    public class SecureServiceShould
    {
        private string _password;
        private ISecureService _secureService;
        public SecureServiceShould()
        {
            _password = "qwerty";
            _secureService = new SecureService(new HashingOptions());
        }

        [Fact]
        public void Hash_PasswordWithThreeDistinctParts()
        {
            var hashedPassword = _secureService.Hash(_password);
            var parts = hashedPassword.Split(".");
            parts.Length.Should().Be(3, "The hashing algorithm hashes them into three distinct parts.");
        }

        [Fact]
        public void Check_CorrectPassword_Verified()
        {
            var hashedPassword = _secureService.Hash(_password);
            var result = _secureService.Check(hashedPassword, _password);
            result.Verified.Should().Be(true, "correct password was supplied");
        }

        [Fact]
        public void Check_InCorrectPassword_NotVerified()
        {
            var hashedPassword = _secureService.Hash(_password);
            var result = _secureService.Check(hashedPassword, "BobSaget");
            result.Verified.Should().Be(false, "correct password was supplied");
        }
    }
}
