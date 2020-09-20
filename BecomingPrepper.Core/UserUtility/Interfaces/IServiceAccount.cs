namespace BecomingPrepper.Core.UserUtility.Interfaces
{
    public interface IServiceAccount
    {
        void UpdatePassword(string accountId, string password);
        void UpdateFamilySize(string accountId, int familySize);
        void UpdateObjective(string accountId, int objective);
        void UpdateEmail(string accountId, string email);
    }
}
