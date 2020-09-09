using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FoodStorageInventoryRepository : IDatabaseCollection<FoodStorageInventoryEntity>
    {
        public Task Add(FoodStorageInventoryEntity t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(FilterDefinition<FoodStorageInventoryEntity> deleteFilter)
        {
            throw new NotImplementedException();
        }

        public Task<FoodStorageInventoryEntity> Get(FilterDefinition<FoodStorageInventoryEntity> queryFilter)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<FoodStorageInventoryEntity> Init(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }

        public Task Update(FilterDefinition<FoodStorageInventoryEntity> queryFilter, UpdateDefinition<FoodStorageInventoryEntity> updateFilter)
        {
            throw new NotImplementedException();
        }
    }
}
