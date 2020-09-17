using System;
using System.Threading.Tasks;
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
            Collection = collection ?? throw new ArgumentNullException("No Collection was provided");
            _logger = exceptionLogger ?? throw new ArgumentNullException("No IExceptionLogger was provided");
        }

        public async Task Add(RecommendedQuantityAmountEntity recommendedQuantityAmountEntity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(FilterDefinition<RecommendedQuantityAmountEntity> deleteFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<RecommendedQuantityAmountEntity> Get(FilterDefinition<RecommendedQuantityAmountEntity> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            RecommendedQuantityAmountEntity entity = null;
            try
            {
                using IAsyncCursor<RecommendedQuantityAmountEntity> asyncCursor = await Collection.FindAsync(filter);
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

        public async Task Update(FilterDefinition<RecommendedQuantityAmountEntity> queryFilter, UpdateDefinition<RecommendedQuantityAmountEntity> updateFilter)
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
