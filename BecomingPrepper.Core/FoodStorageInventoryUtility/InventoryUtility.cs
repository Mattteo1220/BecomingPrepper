using System;
using System.Collections.Generic;
using System.Linq;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Logger;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BecomingPrepper.Core.FoodStorageInventoryUtility
{
    public class InventoryUtility : IInventoryUtility
    {
        public InventoryEntity ItemEntity;
        private IGridFSBucket _bucket;
        private ILogManager _logManager;
        private IGallery _galleryRepo;
        private IRepository<FoodStorageEntity> _inventoryRepository;
        public InventoryUtility(IRepository<FoodStorageEntity> inventoryRepo, IGallery galleryRepo, IGridFSBucket bucket, ILogManager exceptionLog)
        {
            _inventoryRepository = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _galleryRepo = galleryRepo ?? throw new ArgumentNullException(nameof(galleryRepo));
            _bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
            _logManager = exceptionLog ?? throw new ArgumentNullException(nameof(exceptionLog));
        }

        public void AddInventory(FoodStorageEntity entity)
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

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Inventory created for account {entity.AccountId}");
        }

        public void AddInventoryItem(string accountId, InventoryEntity entity)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == accountId);
            var updateFilter = Builders<FoodStorageEntity>.Update.Push(i => i.Inventory, entity);

            try
            {
                _inventoryRepository.Update(filter, updateFilter);
            }
            catch
            {
                return;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Successfully added Inventory Item for account: {accountId}");
        }

        public void DeleteInventory(FoodStorageEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == entity.AccountId);
            try
            {
                _inventoryRepository.Delete(filter);
            }
            catch
            {
                return;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Account {entity.AccountId} had their inventory deleted");
        }

        public FoodStorageEntity GetInventory(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            FoodStorageEntity entity = null;
            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == accountId);

            try
            {
                entity = _inventoryRepository.Get(filter);
            }
            catch
            {
                return null;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Inventory {accountId} retrieved");
            return entity;
        }

        public InventoryEntity GetInventoryItem(string accountId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentNullException(nameof(itemId));
            FoodStorageEntity entity = null;

            try
            {
                entity = GetInventory(accountId);
            }
            catch
            {
                return null;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Item {itemId} for account {accountId} was retrieved");

            return ItemEntity ?? entity.Inventory.FirstOrDefault(i => i.ItemId == itemId);
        }

        public void DeleteInventoryItem(string accountId, string itemId) //Update Method to remove item within inventory
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentNullException(nameof(itemId));

            var arrayFilter = Builders<FoodStorageEntity>.Filter.Where(x => x.AccountId == accountId);
            var update = Builders<FoodStorageEntity>.Update.PullFilter(x => x.Inventory, i => i.ItemId == itemId);
            try
            {
                _inventoryRepository.Update(arrayFilter, update);
            }
            catch
            {
                return;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Account {accountId} had their inventory item {itemId} deleted");
        }

        public void UpdateInventoryItem(string accountId, InventoryEntity entity) //Update Entire item
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var arrayFilter = Builders<FoodStorageEntity>.Filter.And(
                Builders<FoodStorageEntity>.Filter.Where(x => x.AccountId == accountId),
                Builders<FoodStorageEntity>.Filter.ElemMatch(x => x.Inventory, i => i.ItemId == entity.ItemId));
            var arrayUpdate = Builders<FoodStorageEntity>.Update.Combine(Builders<FoodStorageEntity>.Update.Set(i => i.Inventory[-1], entity));
            try
            {
                _inventoryRepository.Update(arrayFilter, arrayUpdate);
            }
            catch
            {
                return;
            }

            _inventoryRepository.Dispose();
            _logManager.LogInformation($"Account {accountId} had their inventory item {entity.ItemId} updated");
        }

        public void GetInventoryImages(List<InventoryEntity> inventory)
        {
            var listOfImageFileInfo = GetFileInfoOfGallery();
            foreach (var item in inventory)
            {
                var objectId = listOfImageFileInfo.Where(i => i.ItemId == item.ItemId).FirstOrDefault()._id;
                try
                {
                    item.Image = _bucket.DownloadAsBytes(objectId);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private List<InventoryImageFileInfoEntity> GetFileInfoOfGallery()
        {
            try
            {
                return _galleryRepo.GetAllImages();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
