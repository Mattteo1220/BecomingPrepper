namespace BecomingPrepper.Core.UserUtility
{
    public interface ILogin
    {
        bool Authenticate(string username, string password);
    }
}
