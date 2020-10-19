namespace BecomingPrepper.Core.UserUtility
{
    public interface ILoginUtility
    {
        string AccountId { get; set; }
        string Email { get; set; }
        bool Authenticate(string username, string password);
        bool IsAuthenticated(string accountId);
    }
}
