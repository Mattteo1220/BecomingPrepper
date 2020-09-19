using System;
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

        public void Add(PrepGuideEntity entity)
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

        public void Delete(FilterDefinition<PrepGuideEntity> deleteFilter)
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

        public PrepGuideEntity Get(FilterDefinition<PrepGuideEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            PrepGuideEntity entity = null;
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

        public void Update(FilterDefinition<PrepGuideEntity> queryFilter, UpdateDefinition<PrepGuideEntity> updateFilter)
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
