namespace BecomingPrepper.Security
{
    public interface ISecureService
    {
        string Hash(string password);
        (bool Verified, bool NeedsUpgrade) Validate(string hash, string password);

    }
}
