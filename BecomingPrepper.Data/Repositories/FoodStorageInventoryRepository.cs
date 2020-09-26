using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FoodStorageInventoryRepository : IRepository<FoodStorageEntity>
    {
        private bool _disposed = false;
        public IMongoCollection<FoodStorageEntity> Collection { get; set; }
        private ILogManager _logger;

        public FoodStorageInventoryRepository(IMongoCollection<FoodStorageEntity> collection, ILogManager logger)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(FoodStorageEntity entity)
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

        public void Delete(FilterDefinition<FoodStorageEntity> deleteFilter)
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

        public FoodStorageEntity Get(FilterDefinition<FoodStorageEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            FoodStorageEntity entity = null;
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

        public void Update(FilterDefinition<FoodStorageEntity> queryFilter, UpdateDefinition<FoodStorageEntity> updateFilter)
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
