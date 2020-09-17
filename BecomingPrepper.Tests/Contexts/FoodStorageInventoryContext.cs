using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class FoodStorageInventoryContext
    {
        public FoodStorageInventoryEntity FoodStorageInventoryEntity { get; set; }
        public IRepository<FoodStorageInventoryEntity> FoodStorageInventoryRepository { get; set; }

        public Func<Task<FoodStorageInventoryEntity>> QueryResult { get; set; }

        public Func<Task> ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
    }
}
