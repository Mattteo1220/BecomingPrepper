using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class UserRepository : IRepository<UserEntity>
    {
        private bool _disposed = false;
        public IMongoCollection<UserEntity> Collection { get; set; }
        private IExceptionLogger _logger;

        public UserRepository(IMongoCollection<UserEntity> collection, IExceptionLogger logger)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            try
            {
                Collection.InsertOne(userEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public UserEntity Get(FilterDefinition<UserEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            UserEntity entity = null;
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

        public void Update(FilterDefinition<UserEntity> queryFilter, UpdateDefinition<UserEntity> updateFilter)
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

        public void Delete(FilterDefinition<UserEntity> deleteFilter)
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
