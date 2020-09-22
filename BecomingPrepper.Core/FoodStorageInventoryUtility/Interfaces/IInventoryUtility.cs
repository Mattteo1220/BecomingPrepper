using BecomingPrepper.Data.Entities;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces
{
    public interface IInventoryUtility
    {
        void AddInventory(FoodStorageInventoryEntity entity);
        void AddInventoryItem(string accountId, InventoryItemEntity entity);
        void DeleteInventoryItem(string itemId);
        void UpdateInventoryItem(string accountId, InventoryItemEntity entity);
        void DeleteInventory(FoodStorageInventoryEntity entity);
        FoodStorageInventoryEntity GetInventory(string accountId);
        InventoryItemEntity GetInventoryItem(string itemId);
    }
}
