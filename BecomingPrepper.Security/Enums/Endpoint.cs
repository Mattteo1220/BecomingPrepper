namespace BecomingPrepper.Security.Enums
{
    public enum Endpoint
    {
        Unknown = 0,
        GetFoodStorageInventory = 1,
        GetInventoryItem = 2,
        AddInventory = 3,
        AddInventoryItem = 4,
        UpdateInventoryItem = 5,
        DeleteInventory = 6,
        DeleteInventoryItem = 7,
        GetImage = 8,
        GetPrepGuide = 9,
        AddTip = 10,
        DeleteTip = 11,
        Welcome = 12,
        GetProducts = 13,
        GetProgress = 14,
        Login = 15,
        Register = 16,
        GetAccountDetails = 17,
        PatchEmail = 18,
        PatchFamilySize = 19,
        PatchObjective = 20,
        ChangePassword = 21
    }
}
