namespace BecomingPrepper.Security
{
    public interface ISecureService
    {
        string Hash(string password);
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}
