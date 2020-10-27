namespace BecomingPrepper.Security
{
    public static class PasswordOptions
    {
        public static int RequiredLength = 10;
        public static int RequiredUniqueChars = 5;
        public static bool RequireNonAlphanumeric = true;
        public static bool RequireLowerCase = true;
        public static bool RequireUpperCase = true;
        public static bool RequireDigit = true;
    }
}
