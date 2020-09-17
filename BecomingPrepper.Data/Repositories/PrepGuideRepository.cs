using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class PrepGuideRepository : IRepository<PrepGuideEntity>
    {
        private bool _disposed = false;
        public IMongoCollection<PrepGuideEntity> Collection { get; set; }
        private IExceptionLogger _logger;

        public PrepGuideRepository(IMongoCollection<PrepGuideEntity> collection, IExceptionLogger exceptionLogger)
        {
            Collection = collection ?? throw new ArgumentNullException("No Collection was provided");
            _logger = exceptionLogger ?? throw new ArgumentNullException("No IExceptionLogger was provided");
        }

        public async Task Add(PrepGuideEntity entity)
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

        public async Task Delete(FilterDefinition<PrepGuideEntity> deleteFilter)
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

        public async Task<PrepGuideEntity> Get(FilterDefinition<PrepGuideEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            PrepGuideEntity entity = null;
            try
            {
                using IAsyncCursor<PrepGuideEntity> asyncCursor = await Collection.FindAsync(queryFilter);
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

        public async Task Update(FilterDefinition<PrepGuideEntity> queryFilter, UpdateDefinition<PrepGuideEntity> updateFilter)
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
