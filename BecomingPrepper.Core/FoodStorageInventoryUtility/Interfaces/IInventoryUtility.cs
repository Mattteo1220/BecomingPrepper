using BecomingPrepper.Data.Entities;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces
{
    public interface IInventoryUtility
    {
        void AddInventory(FoodStorageEntity entity);
        void AddInventoryItem(string accountId, InventoryEntity entity);
        void DeleteInventoryItem(string accountId, string itemId);
        void UpdateInventoryItem(string accountId, InventoryEntity entity);
        void DeleteInventory(FoodStorageEntity entity);
        FoodStorageEntity GetInventory(string accountId);
        InventoryEntity GetInventoryItem(string accountId, string itemId);
    }
}
