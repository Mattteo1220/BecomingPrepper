using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility
{
    class InventoryUtility : IInventoryUtility
    {
        public void AddInventory(FoodStorageInventoryEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddInventoryItem(string accountId, InventoryItemEntity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteInventory(FoodStorageInventoryEntity entity)
        {
            throw new NotImplementedException();
        }

        public FoodStorageInventoryEntity GetInventory(string accountId)
        {
            throw new NotImplementedException();
        }

        public InventoryItemEntity GetInventoryItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public void DeleteInventoryItem(string itemId) //Update Method to remove item within inventory
        {
            throw new NotImplementedException();
        }

        public void UpdateInventoryItem(string accountId, InventoryItemEntity entity) //Update Entire item
        {
            throw new NotImplementedException();
        }
    }
}
