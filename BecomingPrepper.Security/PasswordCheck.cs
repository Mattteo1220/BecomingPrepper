using System.Linq;
using BecomingPrepper.Security.Enums;

namespace BecomingPrepper.Security
{
    public class PasswordCheck
    {
        public static PasswordStrength GetPasswordStrength(string password)
        {
            int score = 0;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
            if (IsStrongPassword(password)) return PasswordStrength.VeryStrong;
            if (HasMinimumLength(password, PasswordOptions.RequiredLength)) score++;
            if (ExceedsMinimumLength(password, PasswordOptions.RequiredLength)) score++;
            if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password)) score++;
            if (HasDigit(password)) score++;
            if (HasSpecialChar(password)) score++;
            if (HasMinimumUniqueChars(password, PasswordOptions.RequiredUniqueChars)) score++;
            return (PasswordStrength)score;
        }

        /// <summary>
        /// Sample password policy implementation:
        /// - minimum 8 characters
        /// - at lease one UC letter
        /// - at least one LC letter
        /// - at least one non-letter char (digit OR special char)
        /// </summary>
        /// <returns></returns>
        public static bool IsStrongPassword(string password)
        {
            return HasMinimumLength(password, PasswordOptions.RequiredLength)
                   && HasUpperCaseLetter(password)
                   && HasLowerCaseLetter(password)
                   && HasDigit(password)
                   && HasSpecialChar(password)
                   && HasMinimumUniqueChars(password, PasswordOptions.RequiredUniqueChars)
                   && ExceedsMinimumLength(password, PasswordOptions.RequiredLength);
        }

        public static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        public static bool ExceedsMinimumLength(string password, int minLength)
        {
            return password.Length > minLength;
        }

        public static bool HasMinimumUniqueChars(string password, int minUniqueChars)
        {
            return password.Distinct().Count() >= minUniqueChars;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one digit
        /// </summary>
        public static bool HasDigit(string password)
        {
            return password.Any(char.IsDigit);
        }

        /// <summary>
        /// Returns TRUE if the password has at least one special character
        /// </summary>
        public static bool HasSpecialChar(string password)
        {
            // return password.Any(c => char.IsPunctuation(c)) || password.Any(c => char.IsSeparator(c)) || password.Any(c => char.IsSymbol(c));
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one uppercase letter
        /// </summary>
        public static bool HasUpperCaseLetter(string password)
        {
            return password.Any(char.IsUpper);
        }

        /// <summary>
        /// Returns TRUE if the password has at least one lowercase letter
        /// </summary>
        public static bool HasLowerCaseLetter(string password)
        {
            return password.Any(char.IsLower);
        }
    }
}
