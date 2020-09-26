using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class FoodStorageInventoryContext
    {
        public FoodStorageEntity FoodStorageInventoryEntity { get; set; }
        public InventoryEntity InventoryItemEntity { get; set; }
        public IRepository<FoodStorageEntity> FoodStorageInventoryRepository { get; set; }

        public Func<FoodStorageEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
        public IInventoryUtility InventoryUtility { get; set; }
    }
}
