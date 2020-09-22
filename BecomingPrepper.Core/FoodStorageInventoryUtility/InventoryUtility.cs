using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility
{
    public class InventoryUtility : IInventoryUtility
    {

        private IExceptionLogger _exceptionLog;
        private IRepository<FoodStorageInventoryEntity> _inventoryRepository;
        public InventoryUtility(IRepository<FoodStorageInventoryEntity> inventoryRepo, IExceptionLogger exceptionLog)
        {
            _inventoryRepository = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _exceptionLog = exceptionLog ?? throw new ArgumentNullException(nameof(exceptionLog));
        }

        public void AddInventory(FoodStorageInventoryEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


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
