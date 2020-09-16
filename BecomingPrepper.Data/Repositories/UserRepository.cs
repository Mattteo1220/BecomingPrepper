using System;
using System.Threading.Tasks;
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

        public UserRepository(IMongoDatabase mongoDatabase, string collection)
        {
            if (mongoDatabase == null) throw new ArgumentNullException(nameof(mongoDatabase));
            Collection = mongoDatabase.GetCollection<UserEntity>(collection);
        }

        public UserRepository(IMongoCollection<UserEntity> collection, IExceptionLogger logger)
        {
            Collection = collection;
            _logger = logger;
        }

        public async Task Add(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            try
            {
                await Collection.InsertOneAsync(userEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public async Task<UserEntity> Get(FilterDefinition<UserEntity> queryFilter)
        {
            if (queryFilter == null) throw new ArgumentNullException(nameof(queryFilter));

            UserEntity entity = null;
            try
            {
                using IAsyncCursor<UserEntity> asyncCursor = await Collection.FindAsync(queryFilter);
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

        public async Task Update(FilterDefinition<UserEntity> queryFilter, UpdateDefinition<UserEntity> updateFilter)
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

        public async Task Delete(FilterDefinition<UserEntity> deleteFilter)
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
