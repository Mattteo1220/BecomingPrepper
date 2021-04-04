using BecomingPrepper.Security;
using BecomingPrepper.Security.Enums;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.Security.SecurityTests
{
    [Trait("Unit", "PasswordCheck")]
    public class PasswordCheckShould
    {
        public PasswordCheckShould()
        {

        }

        [Theory]
        [InlineData("abcdefgh", false)]
        [InlineData("acdkFgdhh", true)]

        public void CheckForUpperCaseLettersInPassword(string passWord, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.HasUpperCaseLetter(passWord));
        }

        [Theory]
        [InlineData("LSKDJFLSDKJF", false)]
        [InlineData("LSDKJFLSDKFJkDIDIE", true)]
        public void CheckForLowerCaseLettersInPassword(string passWord, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.HasLowerCaseLetter(passWord));
        }

        [Theory]
        [InlineData("lks2289dl!kdls", true)]
        [InlineData("lks2289dl@kdls", true)]
        [InlineData("lks2289dl#kdls", true)]
        [InlineData("lks2289dl$kdls", true)]
        [InlineData("lks2289dl%kdls", true)]
        [InlineData("lks2289dl^kdls", true)]
        [InlineData("lks2289dl&kdls", true)]
        [InlineData("lks2289dl*kdls", true)]
        [InlineData("lks2289dl?kdls", true)]
        [InlineData("lks2289dl_kdls", true)]
        [InlineData("lks2289dl~kdls", true)]
        [InlineData("lks2289dl-kdls", true)]
        [InlineData("lks2289dl£kdls", true)]
        [InlineData("lks2289dl(kdls", true)]
        [InlineData("lks2289dl)kdls", true)]
        [InlineData("lks2289dl.kdls", true)]
        [InlineData("lks2289dl,kdls", true)]
        [InlineData("lks2289dlkdls", false)]
        public void CheckForSpecialCharactersInPassword(string passWord, bool expected)
        {
            //Check for the following special characters !@#$%^&*?_~-£()., 
            Assert.Equal(expected, PasswordCheck.HasSpecialChar(passWord));
        }

        [Theory]
        [InlineData("difjcngm8slkd", true)]
        [InlineData("difjcngmslkd", false)]
        public void CheckForDigitsInPassword(string passWord, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.HasDigit(passWord));
        }

        [Theory]
        [InlineData("JaZa2290!@", true)]
        [InlineData("TauTTjTT", false)]
        [InlineData("UIuusdu", true)]
        public void CheckForMinimumUniqueChars(string password, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.HasMinimumUniqueChars(password, PasswordOptions.RequiredUniqueChars));
        }

        [Theory]
        [InlineData("lsdkjfieid", true)]
        [InlineData("lsdkjfieidf", true)]
        [InlineData("d5s6d5d", false)]
        public void CheckForMinimumLengthForPassword(string password, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.HasMinimumLength(password, PasswordOptions.RequiredLength));
        }

        [Theory]
        [InlineData("kdl", false)]
        [InlineData("d", false)]
        [InlineData("dksleisxkfi", true)]
        public void CheckThatPasswordExceedsMinLength(string password, bool expected)
        {
            Assert.Equal(expected, PasswordCheck.ExceedsMinimumLength(password, PasswordOptions.RequiredLength));
        }

        [Theory]
        [InlineData("", PasswordStrength.Blank)]
        [InlineData(null, PasswordStrength.Blank)]
        public void CheckToSeeIfPasswordIsNullOrWhiteSpace(string password, PasswordStrength passwordStrength)
        {
            Assert.Equal(passwordStrength, PasswordCheck.GetPasswordStrength(password));
        }

        [Fact]
        public void CheckToSeeIfPasswordIsStrong()
        {
            Assert.Equal(PasswordStrength.VeryStrong, PasswordCheck.GetPasswordStrength("S0meTh1ng!$tinks"));
        }

        [Theory]
        [InlineData("qwerty", PasswordStrength.VeryWeak)]
        [InlineData("Password", PasswordStrength.Weak)]
        [InlineData("P@ssword", PasswordStrength.Medium)]
        [InlineData("P@ssw0rd", PasswordStrength.Strong)]
        [InlineData("P@s$w0rd!%", PasswordStrength.VeryStrong)]
        public void CheckStrengthOfPassword(string password, PasswordStrength passwordStrength)
        {
            Assert.Equal(passwordStrength, PasswordCheck.GetPasswordStrength(password));
        }
    }
}
