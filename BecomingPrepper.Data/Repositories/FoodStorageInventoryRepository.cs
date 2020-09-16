using System;
using System.Threading.Tasks;
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

        public FoodStorageInventoryRepository(IMongoDatabase mongoDatabase, string collection)
        {
            if (mongoDatabase == null) throw new ArgumentNullException(nameof(mongoDatabase));
            Collection = mongoDatabase.GetCollection<FoodStorageInventoryEntity>(collection);
        }

        public FoodStorageInventoryRepository(IMongoCollection<FoodStorageInventoryEntity> collection, IExceptionLogger logger)
        {
            Collection = collection;
            _logger = logger;
        }

        public async Task Add(FoodStorageInventoryEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            try
            {
                await Collection.InsertOneAsync(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public async Task Delete(FilterDefinition<FoodStorageInventoryEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                await Collection.FindOneAndDeleteAsync(deleteFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public async Task<FoodStorageInventoryEntity> Get(FilterDefinition<FoodStorageInventoryEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            FoodStorageInventoryEntity entity = null;
            try
            {
                using IAsyncCursor<FoodStorageInventoryEntity> asyncCursor = await Collection.FindAsync(queryFilter);
                while (await asyncCursor.MoveNextAsync())
                {
                    foreach (var cursor in asyncCursor.Current)
                    {
                        entity = cursor;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

            return entity;
        }

        public async Task Update(FilterDefinition<FoodStorageInventoryEntity> queryFilter, UpdateDefinition<FoodStorageInventoryEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                await Collection.FindOneAndUpdateAsync(queryFilter, updateFilter);
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
