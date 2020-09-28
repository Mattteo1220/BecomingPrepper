using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;

namespace BecomingPrepper.Tests.Contexts
{
    public class FoodStorageInventoryContext
    {
        public FoodStorageEntity FoodStorageInventoryEntity { get; set; }
        public InventoryEntity InventoryItemEntity { get; set; }
        public IRepository<FoodStorageEntity> FoodStorageInventoryRepository { get; set; }
        public IGalleryFileHelperRepository GalleryFileHelperRepository { get; set; }
        public IGalleryImageHelperRepository GalleryImageHelperRepository { get; set; }

        public Func<FoodStorageEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
        public IInventoryUtility InventoryUtility { get; set; }
    }
}
