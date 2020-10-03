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

namespace BecomingPrepper.Core.FoodStorageInventoryUtility
{
    public class InventoryUtility : IInventoryUtility
    {
        public InventoryEntity ItemEntity;
        private IGalleryImageHelperRepository _imageHelperRepo;
        private ILogManager _logManager;
        private IGalleryFileHelperRepository _galleryRepo;
        private IRepository<FoodStorageEntity> _inventoryRepository;
        public InventoryUtility(IRepository<FoodStorageEntity> inventoryRepo, IGalleryFileHelperRepository galleryRepo, IGalleryImageHelperRepository imageHelperRepo, ILogManager exceptionLog)
        {
            _inventoryRepository = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _galleryRepo = galleryRepo ?? throw new ArgumentNullException(nameof(galleryRepo));
            _imageHelperRepo = imageHelperRepo ?? throw new ArgumentNullException(nameof(imageHelperRepo));
            _logManager = exceptionLog ?? throw new ArgumentNullException(nameof(exceptionLog));
        }

        public void AddInventory(FoodStorageEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _inventoryRepository.Add(entity);

            _logManager.LogInformation($"Inventory created for account {entity.AccountId}");
        }

        public void AddInventoryItem(string accountId, InventoryEntity entity)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == accountId);
            var updateFilter = Builders<FoodStorageEntity>.Update.Push(i => i.Inventory, entity);

            _inventoryRepository.Update(filter, updateFilter);

            _logManager.LogInformation($"Successfully added Inventory Item for account: {accountId}");
        }

        public void DeleteInventory(FoodStorageEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == entity.AccountId);
            _inventoryRepository.Delete(filter);

            _logManager.LogInformation($"Account {entity.AccountId} had their inventory deleted");
        }

        public FoodStorageEntity GetInventory(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            FoodStorageEntity entity = null;
            var filter = Builders<FoodStorageEntity>.Filter.Where(fs => fs.AccountId == accountId);

            entity = _inventoryRepository.Get(filter);
            GetInventoryImages(entity.Inventory);

            _logManager.LogInformation($"Inventory {accountId} retrieved");
            return entity;
        }

        public InventoryEntity GetInventoryItem(string accountId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentNullException(nameof(itemId));
            FoodStorageEntity entity = null;

            entity = GetInventory(accountId);

            _logManager.LogInformation($"Item {itemId} for account {accountId} was retrieved");

            return ItemEntity ?? entity.Inventory.FirstOrDefault(i => i.ItemId == itemId);
        }

        public void DeleteInventoryItem(string accountId, string itemId) //Update Method to remove item within inventory
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentNullException(nameof(itemId));

            var arrayFilter = Builders<FoodStorageEntity>.Filter.Where(x => x.AccountId == accountId);
            var update = Builders<FoodStorageEntity>.Update.PullFilter(x => x.Inventory, i => i.ItemId == itemId);
            _inventoryRepository.Update(arrayFilter, update);

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
            _inventoryRepository.Update(arrayFilter, arrayUpdate);

            _logManager.LogInformation($"Account {accountId} had their inventory item {entity.ItemId} updated");
        }

        public void GetInventoryImages(List<InventoryEntity> inventory)
        {
            var listOfImageFileInfo = GetFileInfoOfGallery();
            if (listOfImageFileInfo != null)
            {
                foreach (var item in inventory)
                {
                    var objectId = listOfImageFileInfo.FirstOrDefault(f => f.ItemId == item.ItemId)._id;
                    var data = _imageHelperRepo.GetImage(objectId);
                    item.Image = data;
                }
            }
        }

        private List<GalleryFileInfoEntity> GetFileInfoOfGallery()
        {
            return _galleryRepo.GetFiles();
        }
    }
}
