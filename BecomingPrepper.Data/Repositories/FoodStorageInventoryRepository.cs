using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FoodStorageInventoryRepository : IRepository<FoodStorageInventoryEntity>
    {
        private bool _disposed = false;
        public IMongoCollection<FoodStorageInventoryEntity> Collection { get; set; }
        private IExceptionLogger _logger;

        public FoodStorageInventoryRepository(IMongoCollection<FoodStorageInventoryEntity> collection, IExceptionLogger logger)
        {
            Collection = collection ?? throw new ArgumentNullException("No Collection was provided");
            _logger = logger ?? throw new ArgumentNullException("No IExceptionLogger was provided");
        }

        public void Add(FoodStorageInventoryEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                Collection.InsertOne(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public void Delete(FilterDefinition<FoodStorageInventoryEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                Collection.FindOneAndDelete(deleteFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public FoodStorageInventoryEntity Get(FilterDefinition<FoodStorageInventoryEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            FoodStorageInventoryEntity entity = null;
            try
            {
                entity = Collection.Find(queryFilter).Limit(1).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

            return entity;
        }

        public void Update(FilterDefinition<FoodStorageInventoryEntity> queryFilter, UpdateDefinition<FoodStorageInventoryEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                Collection.FindOneAndUpdate(queryFilter, updateFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
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
