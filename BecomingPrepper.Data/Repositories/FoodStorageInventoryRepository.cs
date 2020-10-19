using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FoodStorageInventoryRepository : IRepository<FoodStorageEntity>
    {
        private readonly IMongoDatabase _mongoContext;
        private readonly IMongoCollection<FoodStorageEntity> _collection;
        private readonly ILogManager _logger;

        public FoodStorageInventoryRepository(IMongoDatabase mongoContext, ILogManager logger)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<FoodStorageEntity>("Inventory");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(FoodStorageEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _collection.InsertOne(entity);
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
                _collection.FindOneAndDelete(deleteFilter);
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
                entity = _collection.Find(queryFilter).Limit(1).FirstOrDefault();
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
                _collection.FindOneAndUpdate(queryFilter, updateFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}
