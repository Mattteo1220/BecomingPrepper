namespace BecomingPrepper.Core.UserUtility.Interfaces
{
    public interface IServiceAccount
    {
        void UpdatePassword(string password);
        void UpdateFamilySize(int familySize);
        void UpdateObjective(int objective);
        void UpdateEmail(string accountId, string email);
    }
}
