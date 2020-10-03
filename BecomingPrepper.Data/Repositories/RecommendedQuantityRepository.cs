using System;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class RecommendedQuantityRepository : IRepository<RecommendedQuantityAmountEntity>
    {
        private bool _disposed = false;
        private IMongoCollection<RecommendedQuantityAmountEntity> _collection;
        private IMongoDatabase _mongoContext;
        private ILogManager _logger;

        public RecommendedQuantityRepository(IMongoDatabase mongoContext, ILogManager logManager)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _collection = _mongoContext.GetCollection<RecommendedQuantityAmountEntity>("RecommendedQuantities");
            _logger = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        public void Add(RecommendedQuantityAmountEntity recommendedQuantityAmountEntity)
        {
            if (recommendedQuantityAmountEntity == null) throw new ArgumentNullException(nameof(recommendedQuantityAmountEntity));
            try
            {
                _collection.InsertOneAsync(recommendedQuantityAmountEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public void Delete(FilterDefinition<RecommendedQuantityAmountEntity> deleteFilter)
        {
            if (deleteFilter == null) throw new ArgumentNullException(nameof(deleteFilter));

            try
            {
                _collection.FindOneAndDeleteAsync(deleteFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public RecommendedQuantityAmountEntity Get(FilterDefinition<RecommendedQuantityAmountEntity> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            RecommendedQuantityAmountEntity entity = null;
            try
            {
                entity = _collection.Find(filter).Limit(1).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

            return entity;
        }

        public void Update(FilterDefinition<RecommendedQuantityAmountEntity> queryFilter, UpdateDefinition<RecommendedQuantityAmountEntity> updateFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));
            if (updateFilter == null) throw new ArgumentNullException(nameof(updateFilter));

            try
            {
                _collection.FindOneAndUpdateAsync(queryFilter, updateFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}
