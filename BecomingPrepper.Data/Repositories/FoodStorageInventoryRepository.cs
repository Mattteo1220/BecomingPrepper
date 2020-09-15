﻿using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FoodStorageInventoryRepository : IRepository<FoodStorageInventoryEntity>
    {
        private bool _disposed = false;
        public IMongoCollection<FoodStorageInventoryEntity> Collection { get; set; }

        public FoodStorageInventoryRepository(IMongoDatabase mongoDatabase, string collection)
        {
            if (mongoDatabase == null) throw new ArgumentNullException(nameof(mongoDatabase));
            Collection = mongoDatabase.GetCollection<FoodStorageInventoryEntity>(collection);
        }

        public FoodStorageInventoryRepository(IMongoCollection<FoodStorageInventoryEntity> collection)
        {
            Collection = collection;
        }

        public Task Add(FoodStorageInventoryEntity entity)
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

        public Task Update(FilterDefinition<FoodStorageInventoryEntity> queryFilter, UpdateDefinition<FoodStorageInventoryEntity> updateFilter)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Collection = null;
                _disposed = true;
            }
        }
    }
}
