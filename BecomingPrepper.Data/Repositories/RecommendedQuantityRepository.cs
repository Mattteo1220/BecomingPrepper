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
        public IMongoCollection<RecommendedQuantityAmountEntity> Collection { get; set; }
        private IExceptionLogger _logger;

        public RecommendedQuantityRepository(IMongoCollection<RecommendedQuantityAmountEntity> collection, IExceptionLogger exceptionLogger)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _logger = exceptionLogger ?? throw new ArgumentNullException(nameof(exceptionLogger));
        }

        public void Add(RecommendedQuantityAmountEntity recommendedQuantityAmountEntity)
        {
            if (recommendedQuantityAmountEntity == null) throw new ArgumentNullException(nameof(recommendedQuantityAmountEntity));
            try
            {
                Collection.InsertOneAsync(recommendedQuantityAmountEntity);
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
                Collection.FindOneAndDeleteAsync(deleteFilter);
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
                entity = Collection.Find(filter).Limit(1).FirstOrDefault();
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
                Collection.FindOneAndUpdateAsync(queryFilter, updateFilter);
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
