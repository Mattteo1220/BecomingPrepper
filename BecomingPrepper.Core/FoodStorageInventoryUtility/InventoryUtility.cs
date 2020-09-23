using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility
{
    public class InventoryUtility : IInventoryUtility
    {

        private ILogManager _exceptionLog;
        private IRepository<FoodStorageInventoryEntity> _inventoryRepository;
        public InventoryUtility(IRepository<FoodStorageInventoryEntity> inventoryRepo, ILogManager exceptionLog)
        {
            _inventoryRepository = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _exceptionLog = exceptionLog ?? throw new ArgumentNullException(nameof(exceptionLog));
        }

        public void AddInventory(FoodStorageInventoryEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _inventoryRepository.Add(entity);
            }
            catch
            {
                return;
            }

            _exceptionLog.LogInformation($"Inventory created for account {entity.AccountId}");
        }

        public void AddInventoryItem(string accountId, InventoryItemEntity entity)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var filter = Builders<FoodStorageInventoryEntity>.Filter.Where(fs => fs.AccountId == accountId);
            var updateFilter = Builders<FoodStorageInventoryEntity>.Update.Push(i => i.Inventory, entity);

            try
            {
                _inventoryRepository.Update(filter, updateFilter);
            }
            catch
            {
                return;
            }

            _exceptionLog.LogInformation($"Successfully added Inventory Item for account: {accountId}");
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
