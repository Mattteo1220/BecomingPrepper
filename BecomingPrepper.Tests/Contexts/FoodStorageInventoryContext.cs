using System;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class FoodStorageInventoryContext
    {
        public FoodStorageInventoryEntity FoodStorageInventoryEntity { get; set; }
        public InventoryItemEntity InventoryItemEntity { get; set; }
        public IRepository<FoodStorageInventoryEntity> FoodStorageInventoryRepository { get; set; }

        public Func<FoodStorageInventoryEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
        public IInventoryUtility InventoryUtility { get; set; }
    }
}
