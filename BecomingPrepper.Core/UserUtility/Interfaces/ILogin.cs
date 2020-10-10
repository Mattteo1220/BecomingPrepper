namespace BecomingPrepper.Core.UserUtility
{
    public interface ILogin
    {
        string AccountId { get; set; }
        string Email { get; set; }
        bool Authenticate(string username, string password);
        bool IsAuthenticated(string accountId);
    }
}
